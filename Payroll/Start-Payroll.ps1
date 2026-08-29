# Start-PayrollHCC.ps1 - reliable startup for the Payroll_HCC2 HR solution.
# Safe to run any time: does nothing if the app is already up.
# Steps: ensure SQL Server -> ensure build output -> launch IIS Express -> health-check.
# NOTE: keep this file pure ASCII (no em-dashes/curly quotes) - PS 5.1 reads it as ANSI.

$ErrorActionPreference = 'Stop'
$Root      = Split-Path -Parent $MyInvocation.MyCommand.Path
$SiteDir   = Join-Path $Root 'Payroll_HCC2\Payroll-HCC'
$Sln       = Join-Path $Root 'Payroll_HCC2\Payroll-HCC.sln'
$SiteDll   = Join-Path $SiteDir 'bin\Payroll-HCC.dll'
$Port      = 7637
$Url       = "http://localhost:$Port/payroll/Account/Login?local=1"   # app is hosted under /payroll (ERP reverse-proxy prefix)
$IisConfig = Join-Path $Root 'Payroll_HCC2\iisexpress\applicationhost.config'
$IisExpress = 'C:\Program Files\IIS Express\iisexpress.exe'
$MsBuild    = 'D:\Programs\VS Applications\MSBuild\Current\Bin\MSBuild.exe'
$LogFile    = Join-Path $Root 'Start-PayrollHCC.log'
$WarmupSeconds = 300   # ASP.NET first-request compile is slow on this machine (2-4 min)

function Log($msg) {
    $line = "{0}  {1}" -f (Get-Date -Format 'yyyy-MM-dd HH:mm:ss'), $msg
    Add-Content -Path $LogFile -Value $line -Encoding utf8
    Write-Host $line
}

function Test-PortListening {
    $null -ne (Get-NetTCPConnection -LocalPort $Port -State Listen -ErrorAction SilentlyContinue)
}

function Test-App([int]$TimeoutSec = 10) {
    try {
        $r = Invoke-WebRequest -Uri $Url -UseBasicParsing -TimeoutSec $TimeoutSec
        return ($r.StatusCode -eq 200)
    } catch { return $false }
}

# Waits until the app answers HTTP 200 or the deadline passes.
function Wait-App([int]$Seconds) {
    $deadline = (Get-Date).AddSeconds($Seconds)
    while ((Get-Date) -lt $deadline) {
        if (Test-App 15) { return $true }
        Start-Sleep -Seconds 3
    }
    return $false
}

Log "=== Start-PayrollHCC invoked ==="

# 0. Something already listening on the port? Give it the full warm-up window
#    before declaring it dead (IIS Express idle-unloads the app; the next
#    request re-warms it and can take minutes).
if (Test-PortListening) {
    Log "Port $Port already listening - waiting for the app to answer (warm-up)..."
    if (Wait-App $WarmupSeconds) { Log "App already up at $Url - nothing to do."; exit 0 }
    Log "Listener on port $Port never answered - restarting it."
    Get-CimInstance Win32_Process -Filter "Name = 'iisexpress.exe'" |
        Where-Object { $_.CommandLine -like ("*" + $Port + "*") } |
        ForEach-Object { try { Stop-Process -Id $_.ProcessId -Force -Confirm:$false } catch {} }
    Start-Sleep -Seconds 2
}

# 1. Ensure SQL Server instance MSSQLSERVER01 is running.
$svc = Get-Service 'MSSQL$MSSQLSERVER01' -ErrorAction SilentlyContinue
if ($null -eq $svc) {
    Log "WARNING: SQL service MSSQL`$MSSQLSERVER01 not found - app pages needing the DB will fail."
} elseif ($svc.Status -ne 'Running') {
    Log "SQL service is $($svc.Status) - starting it..."
    try {
        Start-Service 'MSSQL$MSSQLSERVER01'
        $svc.WaitForStatus('Running', (New-TimeSpan -Seconds 60))
        Log "SQL service started."
    } catch {
        Log "WARNING: could not start SQL service ($($_.Exception.Message)). Continuing - it may need admin rights."
    }
} else {
    Log "SQL service running."
}

# 2. Ensure the site is built.
if (-not (Test-Path $SiteDll)) {
    Log "Build output missing - building solution..."
    if (-not (Test-Path $MsBuild)) { Log "ERROR: MSBuild not found at $MsBuild"; exit 1 }
    & $MsBuild $Sln /p:Configuration=Debug /nologo /v:m |
        Out-File -FilePath (Join-Path $Root 'Start-PayrollHCC.build.log') -Encoding utf8
    if ($LASTEXITCODE -ne 0 -or -not (Test-Path $SiteDll)) {
        Log "ERROR: build failed - see Start-PayrollHCC.build.log"; exit 1
    }
    Log "Build succeeded."
} else {
    Log "Build output present."
}

# 3. Launch IIS Express detached (survives this script/console exiting).
if (-not (Test-Path $IisExpress)) { Log "ERROR: IIS Express not found at $IisExpress"; exit 1 }
$p = Start-Process -FilePath $IisExpress `
        -ArgumentList "/config:`"$IisConfig`"", "/site:PayrollHCC" `
        -WindowStyle Hidden -PassThru
Log "IIS Express launched (PID $($p.Id)). Waiting for first response (ASP.NET warm-up, up to $WarmupSeconds s)..."

# 4. Health-check.
$deadline = (Get-Date).AddSeconds($WarmupSeconds)
while ((Get-Date) -lt $deadline) {
    if (Test-App 15) { Log "SUCCESS: app is up at $Url"; exit 0 }
    if ($p.HasExited) { Log "ERROR: IIS Express exited (code $($p.ExitCode))."; exit 1 }
    Start-Sleep -Seconds 3
}
Log "ERROR: app did not respond within $WarmupSeconds s. Check IIS Express (PID $($p.Id))."
exit 1
