' Runs the Payroll HR solution startup script hidden (no console window).
' Copy this file into shell:startup to auto-start the app at logon.
CreateObject("WScript.Shell").Run "powershell.exe -NoProfile -ExecutionPolicy Bypass -File ""C:\Users\Adnan Ahmed\Pictures\Payroll\Start-PayrollHCC.ps1""", 0, False
