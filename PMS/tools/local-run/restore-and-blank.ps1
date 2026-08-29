<#
    restore-and-blank.ps1
    ------------------------------------------------------------------------------------------
    Turns a full database backup into a structure-only backup: same schema, zero rows, small
    enough to move around and restore anywhere.

    Written 2026-08-13 for F:\DHA_live_20260423_1424 (DHA_Live, SQL Server 2022).

    WHAT IT DOES
      1  Reads the backup header and refuses to start if the target instance is older than the
         instance the backup came from. SQL Server backups only ever restore forwards.
      2  Checks free space on each target drive against the file sizes in the backup.
      3  Restores the backup, moving each file to the drive with room for it.
      4  Runs blank-database.sql — empties every table, keeps the schema.
      5  Shrinks the data and log files down to the now-tiny footprint.
      6  Backs up the emptied database, compressed and checksummed.
      7  Verifies the new backup and prints its size.
      8  Optionally drops the working database to give the ~310 GB back.

    THE SOURCE BACKUP IS ONLY EVER READ. This script never writes to it.

    USAGE
      .\restore-and-blank.ps1 -SqlInstance ".\SQL2022"
      .\restore-and-blank.ps1 -SqlInstance ".\SQL2022" -DropWorkingDbWhenDone
    ------------------------------------------------------------------------------------------ #>

[CmdletBinding()]
param(
    [string] $SqlInstance   = '.\MSSQLSERVER01',   # the SQL Server 2022 named instance on this machine
    [string] $BackupPath    = 'F:\DHA_live_20260423_1424',
    [string] $TargetDb      = 'DHA_Blank',

    # One file per drive — the three will not fit together on any single volume.
    [string] $MdfDir        = 'E:\SQLWork',
    [string] $NdfDir        = 'F:\SQLWork',
    [string] $LogDir        = 'D:\SQLWork',

    [string] $OutputBak     = 'F:\DHA_Blank_Structure.bak',
    [switch] $DropWorkingDbWhenDone
)

$ErrorActionPreference = 'Stop'
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$blankSql  = Join-Path $scriptDir 'blank-database.sql'

function Say  ([string]$m) { Write-Host "  $m" }
function Head ([string]$m) { Write-Host ''; Write-Host "== $m" -ForegroundColor Cyan }
function Sql  ([string]$q, [int]$timeout = 0) {
    $out = sqlcmd -S $SqlInstance -E -b -Q $q -t $timeout 2>&1
    if ($LASTEXITCODE -ne 0) { throw ("sqlcmd failed:`n" + ($out -join "`n")) }
    return $out
}

$started = Get-Date
Head "restore-and-blank  ·  $($started.ToString('yyyy-MM-dd HH:mm:ss'))"
Say "instance : $SqlInstance"
Say "backup   : $BackupPath"
Say "target   : $TargetDb"

if (-not (Test-Path $BackupPath)) { throw "Backup not found: $BackupPath" }
if (-not (Test-Path $blankSql))   { throw "blank-database.sql not found beside this script." }

# Same protected list as blank-database.sql. Step 3 restores WITH REPLACE and step 4 truncates
# every table, so a wrong -TargetDb destroys whatever is already under that name. Extend this
# list, never shorten it.
$protected = @('master', 'model', 'msdb', 'tempdb', 'DHA_Live', 'PMS_Local', 'test_dha_land_management')
if ($protected -contains $TargetDb) {
    throw "Refusing to run against [$TargetDb] - it is on the protected list. Pick a scratch database name."
}

# ── 1 · Version check ─────────────────────────────────────────────────────────────────────────
Head '1 · Version compatibility'

$hdrFile = Join-Path $env:TEMP 'rb_header.txt'
sqlcmd -S $SqlInstance -E -b -Q "RESTORE HEADERONLY FROM DISK='$BackupPath'" -o $hdrFile -s'|' -w 8000
if ($LASTEXITCODE -ne 0) { throw "Could not read the backup header. Is $SqlInstance reachable?" }

$hl   = Get-Content $hdrFile
$cols = ($hl[0] -split '\|') | ForEach-Object { $_.Trim() }
$vals = ($hl[2] -split '\|') | ForEach-Object { $_.Trim() }
$h    = @{}
for ($i = 0; $i -lt $cols.Count; $i++) { $h[$cols[$i]] = $vals[$i] }

$srcMajor = [int]$h['SoftwareVersionMajor']
$srcDb    = $h['DatabaseName']
$tgtMajor = [int]((Sql "SET NOCOUNT ON; SELECT LEFT(CAST(SERVERPROPERTY('ProductVersion') AS varchar(20)), CHARINDEX('.', CAST(SERVERPROPERTY('ProductVersion') AS varchar(20))) - 1)" | Where-Object { $_ -match '^\s*\d+\s*$' } | Select-Object -First 1).Trim())

Say "backup made on : SQL Server major $srcMajor  (database '$srcDb', compat $($h['CompatibilityLevel']))"
Say "target runs    : SQL Server major $tgtMajor"

if ($tgtMajor -lt $srcMajor) {
    throw @"
Cannot restore. The backup came from SQL Server major version $srcMajor and this instance is $tgtMajor.
SQL Server backups restore forwards only — there is no downgrade path, no compatibility flag.
Install SQL Server $(if ($srcMajor -eq 16) {'2022'} else {"major $srcMajor"}) or newer and point -SqlInstance at it.
"@
}
Say 'OK — target is new enough.'

# ── 2 · Space check ───────────────────────────────────────────────────────────────────────────
Head '2 · Disk space'

$flFile = Join-Path $env:TEMP 'rb_filelist.txt'
sqlcmd -S $SqlInstance -E -b -Q "SET NOCOUNT ON; RESTORE FILELISTONLY FROM DISK='$BackupPath'" -o $flFile -s'|' -w 8000
$fl = Get-Content $flFile | Where-Object { $_ -match '\|' -and $_ -notmatch '^-+\|' }

$files = @()
$fcols = ($fl[0] -split '\|') | ForEach-Object { $_.Trim() }
foreach ($line in $fl[1..($fl.Count - 1)]) {
    $fv = ($line -split '\|') | ForEach-Object { $_.Trim() }
    if ($fv.Count -lt $fcols.Count) { continue }
    $r = @{}
    for ($i = 0; $i -lt $fcols.Count; $i++) { $r[$fcols[$i]] = $fv[$i] }
    if (-not $r['LogicalName']) { continue }
    $files += [pscustomobject]@{
        Logical = $r['LogicalName']
        Type    = $r['Type']
        Bytes   = [int64]$r['Size']
        GB      = [math]::Round([int64]$r['Size'] / 1GB, 1)
    }
}

# Assign each file a home: first data file → Mdf, later data files → Ndf, log → Log.
$dataSeen = 0
foreach ($f in $files) {
    if ($f.Type -eq 'L') { $dir = $LogDir }
    else { $dataSeen++; $dir = if ($dataSeen -eq 1) { $MdfDir } else { $NdfDir } }
    $ext = switch ($f.Type) { 'L' { '.ldf' } default { if ($dataSeen -eq 1) { '.mdf' } else { '.ndf' } } }
    Add-Member -InputObject $f -NotePropertyName Target -NotePropertyValue (Join-Path $dir ($f.Logical + $ext))
    Say ("{0,-18} {1,7} GB  ->  {2}" -f $f.Logical, $f.GB, $f.Target)
}

# Drop any existing copy of the target before measuring space.
#
# RESTORE does NOT reuse the files of the database it replaces: it demands the full size free
# again, on top of what the old files already occupy, and fails its own planning check before
# writing anything. With 310 GB of files that shortfall is unfixable on this machine, so the old
# copy has to go first.
#
# Dropping unconditionally is safe here because this script only ever builds a throwaway scratch
# database from a backup it opens read-only. There is nothing in the target worth keeping -- and
# the protected-name gate above is what stops that reasoning being applied to a real database.
$exists = (Sql "SET NOCOUNT ON; SELECT state_desc FROM sys.databases WHERE name = N'$TargetDb';" |
           Where-Object { $_ -match '^\s*[A-Z_]+\s*$' } |
           Select-Object -First 1)
if ($exists) {
    Say "found an existing [$TargetDb] ($($exists.Trim())). Dropping it to release its files."
    # Kick off any open sessions first. This fails on a database still in RESTORING, which has no
    # sessions to kick off anyway, so the failure is expected and ignored.
    try { Sql "ALTER DATABASE [$TargetDb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;" 0 | Out-Null } catch { }
    Sql "DROP DATABASE [$TargetDb];" 0 | Out-Null
    Start-Sleep -Seconds 3   # let the filesystem catch up before the free-space numbers are read
    Say 'dropped.'
}

foreach ($grp in $files | Group-Object { (Split-Path $_.Target -Qualifier) }) {
    $drive  = $grp.Name.TrimEnd(':')
    $need   = ($grp.Group | Measure-Object Bytes -Sum).Sum
    $freeGB = [math]::Round((Get-PSDrive $drive).Free / 1GB, 1)
    $needGB = [math]::Round($need / 1GB, 1)
    if ($need -gt (Get-PSDrive $drive).Free) {
        throw "Drive ${drive}: needs $needGB GB but only $freeGB GB is free."
    }
    Say ("drive {0}:  needs {1,7} GB, free {2,7} GB  OK" -f $drive, $needGB, $freeGB)
}

foreach ($d in @($MdfDir, $NdfDir, $LogDir)) {
    if (-not (Test-Path $d)) { New-Item -ItemType Directory -Force -Path $d | Out-Null; Say "created $d" }
}

$ifi = (Sql "SET NOCOUNT ON; SELECT instant_file_initialization_enabled FROM sys.dm_server_services WHERE servicename LIKE '%SQL Server (%'") -match '^\s*[YN]\s*$'
if ($ifi -and $ifi[0].Trim() -eq 'N') {
    Write-Warning 'Instant File Initialization is OFF. SQL Server will zero-write every byte of the data files before restoring — expect this to add roughly an hour. Grant "Perform Volume Maintenance Tasks" to the SQL Server service account and restart the service to avoid it.'
}

# ── 3 · Restore ───────────────────────────────────────────────────────────────────────────────
Head '3 · Restore  (the long step — 310 GB of files to lay down)'

$moves = ($files | ForEach-Object { "MOVE N'$($_.Logical)' TO N'$($_.Target)'" }) -join ", `n     "
$restore = @"
RESTORE DATABASE [$TargetDb]
FROM DISK = N'$BackupPath'
WITH $moves,
     REPLACE, STATS = 5;
"@

Say 'running…'
$t0 = Get-Date
sqlcmd -S $SqlInstance -E -b -Q $restore -t 0
if ($LASTEXITCODE -ne 0) { throw 'RESTORE failed — see the output above.' }
Say ("restored in {0:n1} min" -f ((Get-Date) - $t0).TotalMinutes)

$before = Sql "SET NOCOUNT ON; USE [$TargetDb]; SELECT CAST(SUM(size) * 8.0 / 1048576 AS decimal(10,1)) FROM sys.database_files;"
Say ("database files now: " + (($before | Where-Object { $_ -match '\d' } | Select-Object -First 1).Trim()) + ' GB')

# ── 4 · Empty it ──────────────────────────────────────────────────────────────────────────────
Head '4 · Emptying every table'

$tmpSql = Join-Path $env:TEMP 'rb_blank.sql'
(Get-Content $blankSql -Raw) -replace "DECLARE @DatabaseName sysname = N'DHA_Blank';", "DECLARE @DatabaseName sysname = N'$TargetDb';" |
    Set-Content $tmpSql -Encoding utf8

# -I turns QUOTED_IDENTIFIER on. sqlcmd leaves it OFF by default, and this database has indexed
# views and filtered indexes, which make every INSERT/DELETE against them fail with error 1934
# without it. blank-database.sql sets it too; both are kept so neither path can regress.
$t0 = Get-Date
sqlcmd -S $SqlInstance -E -b -I -d $TargetDb -i $tmpSql -t 0
if ($LASTEXITCODE -ne 0) { throw 'blank-database.sql failed — see the output above.' }
Say ("emptied in {0:n1} min" -f ((Get-Date) - $t0).TotalMinutes)

# ── 5 · Shrink ────────────────────────────────────────────────────────────────────────────────
Head '5 · Shrinking the files'

$shrink = @"
SET NOCOUNT ON;
USE [$TargetDb];
CHECKPOINT;   -- lets the log truncate under SIMPLE recovery before we try to shrink it
DECLARE @f sysname, @s nvarchar(400);
DECLARE c CURSOR LOCAL FAST_FORWARD FOR SELECT name FROM sys.database_files;
OPEN c; FETCH NEXT FROM c INTO @f;
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @s = N'DBCC SHRINKFILE (' + QUOTENAME(@f, '''') + N', 64) WITH NO_INFOMSGS;';
    BEGIN TRY EXEC sys.sp_executesql @s; END TRY
    BEGIN CATCH PRINT '  could not shrink ' + @f + ' — ' + ERROR_MESSAGE(); END CATCH
    FETCH NEXT FROM c INTO @f;
END
CLOSE c; DEALLOCATE c;
SELECT name, CAST(size * 8.0 / 1024 AS decimal(10,1)) AS SizeMB FROM sys.database_files;
"@
sqlcmd -S $SqlInstance -E -b -Q $shrink -t 0 -s'|' -W

# ── 6 · Back it up ────────────────────────────────────────────────────────────────────────────
Head '6 · Writing the structure-only backup'

if (Test-Path $OutputBak) {
    Say "removing previous output $OutputBak"
    Remove-Item $OutputBak -Force
}

Sql @"
BACKUP DATABASE [$TargetDb]
TO DISK = N'$OutputBak'
WITH COMPRESSION, CHECKSUM, INIT, STATS = 10,
     NAME = N'$TargetDb — structure only, zero rows',
     DESCRIPTION = N'Schema-only copy of $srcDb. Every user table truncated; schema, keys, indexes, procedures and permissions intact.';
"@ 0 | Out-Null

# ── 7 · Verify ────────────────────────────────────────────────────────────────────────────────
Head '7 · Verifying'

Sql "RESTORE VERIFYONLY FROM DISK = N'$OutputBak' WITH CHECKSUM;" 0 | ForEach-Object { Say $_ }

$outGB = [math]::Round((Get-Item $OutputBak).Length / 1GB, 3)
$srcGB = [math]::Round((Get-Item $BackupPath).Length / 1GB, 3)

# ── 8 · Optional cleanup ──────────────────────────────────────────────────────────────────────
if ($DropWorkingDbWhenDone) {
    Head '8 · Dropping the working database'
    Sql "ALTER DATABASE [$TargetDb] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [$TargetDb];" 0 | Out-Null
    Say 'dropped — disk reclaimed.'
} else {
    Head '8 · Working database kept'
    Say "[$TargetDb] is still on $SqlInstance and still occupying disk."
    Say "Re-run with -DropWorkingDbWhenDone, or drop it by hand, to reclaim the space."
}

Head 'Done'
Say ("source backup : {0,8} GB   {1}" -f $srcGB, $BackupPath)
Say ("blank backup  : {0,8} GB   {1}" -f $outGB, $OutputBak)
Say ("elapsed       : {0:n1} min" -f ((Get-Date) - $started).TotalMinutes)
Write-Host ''
Say "Restore it anywhere with:"
Say "  RESTORE DATABASE [YourDb] FROM DISK = N'$OutputBak' WITH MOVE ... , RECOVERY;"
Say "Target must be SQL Server major $srcMajor or newer."
