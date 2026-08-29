# Start-EnterpriseSolution.ps1 - starts the whole ERP suite (PMS :5217, LIMS :8000, Payroll :7637).
# Idempotent: each app is skipped when its port already listens. Pure ASCII (PS 5.1 reads it as ANSI).
$ErrorActionPreference = 'Continue'
$Root    = Split-Path -Parent $MyInvocation.MyCommand.Path
$LogFile = Join-Path $Root 'Start-EnterpriseSolution.log'
function Log($m) { $l = "{0}  {1}" -f (Get-Date -Format 'yyyy-MM-dd HH:mm:ss'), $m; Add-Content $LogFile $l -Encoding utf8; Write-Host $l }
function Listening([int]$p) { $null -ne (Get-NetTCPConnection -LocalPort $p -State Listen -ErrorAction SilentlyContinue) }
function WaitPort([int]$p, [int]$sec) { $d = (Get-Date).AddSeconds($sec); while ((Get-Date) -lt $d) { if (Listening $p) { return $true }; Start-Sleep 2 }; return $false }

Log "=== Start-EnterpriseSolution invoked ==="

# SQL Server first (both PMS and Payroll use MSSQLSERVER01)
$svc = Get-Service 'MSSQL$MSSQLSERVER01' -ErrorAction SilentlyContinue
if ($svc -and $svc.Status -ne 'Running') { try { Start-Service $svc.Name; $svc.WaitForStatus('Running', (New-TimeSpan -Seconds 60)); Log "SQL started." } catch { Log "WARN: SQL not started: $($_.Exception.Message)" } }

# LIMS (Laravel dev server)
if (Listening 8000) { Log "LIMS already on :8000." } else {
    Start-Process -FilePath 'C:\PHP\php.exe' -ArgumentList 'artisan','serve','--host=127.0.0.1','--port=8000' -WorkingDirectory (Join-Path $Root 'LMIS') -WindowStyle Hidden
    Log "LIMS launched."
}

# PMS (ASP.NET Core; dotnet run builds if needed - allow a few minutes cold)
if (Listening 5217) { Log "PMS already on :5217." } else {
    Start-Process -FilePath 'dotnet' -ArgumentList 'run','--project','HRMS_Web\HRMS_Web.csproj','--urls','http://localhost:5217' -WorkingDirectory (Join-Path $Root 'PMS') -WindowStyle Hidden
    Log "PMS launched."
}

# Payroll (its own idempotent script handles IIS Express + warm-up)
$payroll = 'C:\Users\Adnan Ahmed\Pictures\Payroll\Start-PayrollHCC.ps1'
if (Test-Path $payroll) { & powershell.exe -NoProfile -ExecutionPolicy Bypass -File $payroll | Out-Null; Log "Payroll script exit $LASTEXITCODE." } else { Log "WARN: $payroll missing." }

if (WaitPort 8000 30) { Log "LIMS up." } else { Log "WARN: LIMS not listening after 30 s." }
if (WaitPort 5217 240) { Log "PMS up." } else { Log "WARN: PMS not listening after 240 s." }
Log "Done: http://localhost:5217"
