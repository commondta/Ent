@echo off
rem Double-click launcher for the Payroll HR solution.
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Start-PayrollHCC.ps1"
if errorlevel 1 (
  echo.
  echo Startup FAILED - see Start-PayrollHCC.log
  pause
  exit /b 1
)
start "" http://localhost:7637/
