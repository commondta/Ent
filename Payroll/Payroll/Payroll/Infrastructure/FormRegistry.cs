using System;
using System.Collections.Generic;
using System.Linq;

namespace Payroll_HCC.Infrastructure
{
    /// <summary>One screen of the application.</summary>
    public class FormInfo
    {
        public string Key { get; set; }          // permission key, e.g. "Payroll.Processing"
        public string Title { get; set; }        // display name
        public string LegacyTitle { get; set; }  // the name it had before the restructuring (kept for search)
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Icon { get; set; }         // icon name from Icons
        public ModuleInfo Module { get; set; }
        public string Url { get { return "/" + Controller + "/" + Action; } }
    }

    /// <summary>A functional module grouping several forms.</summary>
    public class ModuleInfo
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public List<FormInfo> Forms { get; private set; }
        public ModuleInfo() { Forms = new List<FormInfo>(); }
    }

    /// <summary>
    /// Single source of truth for navigation, permissions, search and breadcrumbs.
    /// Order here is the order in the sidebar.
    /// </summary>
    public static class FormRegistry
    {
        public static readonly List<ModuleInfo> Modules = new List<ModuleInfo>();
        static readonly Dictionary<string, FormInfo> byKey = new Dictionary<string, FormInfo>(StringComparer.OrdinalIgnoreCase);
        static readonly Dictionary<string, FormInfo> byRoute = new Dictionary<string, FormInfo>(StringComparer.OrdinalIgnoreCase);

        static FormRegistry()
        {
            ModuleInfo m;

            m = Module("Home", "Home", "home");
            Form(m, "Home.Dashboard", "Dashboard", "Home", "Home", "Index", "dashboard");

            m = Module("Employees", "Employee Management", "people");
            Form(m, "Employees.Companies", "Companies", "Company", "Master", "Company", "building");
            Form(m, "Employees.Profiles", "Employee Profiles", "Employees", "Master", "Employees", "person-badge");
            Form(m, "Employees.Categories", "Employee Categories", "Employee Category Master", "Master", "EmployeeCategoryMaster", "tags");
            Form(m, "Employees.Departments", "Departments", "Department Setup", "Master", "DepartmentSetup", "diagram");

            m = Module("Setup", "Compensation & Payroll Setup", "sliders");
            Form(m, "Setup.PayElements", "Pay Elements", "Pay Elements", "Master", "PayElements", "list-check");
            Form(m, "Setup.PayrollPeriods", "Payroll Periods", "Pay Period", "Master", "PayPeriod", "calendar-range");
            Form(m, "Setup.BonusSetup", "Bonus Setup", "Bonus Master", "Master", "BonusMaster", "gift");
            Form(m, "Setup.PayrollFormulas", "Payroll Formulas", "Formula Master", "Master", "FormulaMaster", "function");
            Form(m, "Setup.PayrollTaxRules", "Payroll Tax Rules", "Tax Formula Calculation", "Master", "TaxFormulaCalculation", "percent");
            Form(m, "Setup.EOBISetup", "EOBI Setup", "EOBI Setup", "Master", "EOBIsetup", "shield");

            m = Module("Benefits", "Leave & Benefits Setup", "heart-pulse");
            Form(m, "Benefits.LeaveTypes", "Leave Types", "Leave Master", "Master", "LeaveMaster", "calendar-x");
            Form(m, "Benefits.LoanSetup", "Loan Setup", "Loan Master", "Master", "LoanMaster", "bank");
            Form(m, "Benefits.OvertimeRules", "Overtime Rules", "Overtime Master", "Master", "OvertimeMaster", "clock-history");

            m = Module("Attendance", "Time & Attendance", "clock");
            Form(m, "Attendance.Daily", "Daily Attendance", "Daily Attendance Sheet", "Transaction", "DailyAttendanceSheet", "calendar-day");
            Form(m, "Attendance.Monthly", "Monthly Attendance", "Monthly Attendance Sheet", "Transaction", "MonthlyAttendanceSheet", "calendar-month");

            m = Module("Payroll", "Payroll Transactions", "cash-stack");
            Form(m, "Payroll.MonthlyAdditions", "Monthly Additions", "Monthly Addition", "Transaction", "MonthlyAddition", "plus-square");
            Form(m, "Payroll.MonthlyDeductions", "Monthly Deductions", "Monthly Deduction", "Transaction", "MonthlyDeduction", "dash-square");
            Form(m, "Payroll.PaymentsDeductions", "Payroll Payments & Deductions", "Payments and Deductions", "Transaction", "PaymentsAndDeductions", "arrow-left-right");
            Form(m, "Payroll.Processing", "Payroll Processing", "Payroll Process", "Transaction", "PayrollProcess", "gear-play");

            m = Module("Overtime", "Overtime Management", "hourglass");
            Form(m, "Overtime.Processing", "Overtime Processing", "Overtime Process", "Transaction", "OvertimeProcess", "gear-play");
            Form(m, "Overtime.Formulas", "Overtime Formulas", "Overtime Formula", "Transaction", "OvertimeFormula", "function");

            m = Module("Leave", "Leave Management", "calendar-check");
            Form(m, "Leave.Requests", "Leave Requests", "Leave Application", "Transaction", "LeaveApplication", "envelope-paper");
            Form(m, "Leave.Settlement", "Leave Settlement", "Leave Settlement", "Transaction", "LeaveSettlement", "check-square");

            m = Module("Loans", "Loan & Advances", "wallet");
            Form(m, "Loans.Requests", "Loan & Salary Advance Requests", "Loan Application", "Transaction", "LoanApplication", "cash-coin");

            m = Module("Reports", "Employee Reports", "file-text");
            Form(m, "Reports.Personal", "Employee Personal Details", "Personal Detail", "Reports", "PersonalDetail", "person-lines");
            Form(m, "Reports.Address", "Employee Address Details", "Address Detail", "Reports", "AddressDetail", "geo");
            Form(m, "Reports.Bank", "Employee Bank Details", "Bank Detail", "Reports", "BankDetail", "bank");
            Form(m, "Reports.Job", "Employee Job Details", "Job Detail", "Reports", "JobDetail", "briefcase");

            m = Module("Security", "Security & Administration", "shield-lock");
            Form(m, "Security.Users", "User Management", "Users", "Security", "Users", "person-gear");
            Form(m, "Security.Roles", "Roles & Permissions", "Roles", "Security", "Roles", "key");
            Form(m, "Security.Approvals", "Approvals", "Approvals", "Security", "Approvals", "check-circle");
            Form(m, "Security.ApprovalSetup", "Approval Setup", "Approval Configuration", "Security", "ApprovalSetup", "sliders");
            Form(m, "Security.ActivityLog", "Activity Log", "Activity Log", "Security", "ActivityLog", "activity");
        }

        static ModuleInfo Module(string key, string title, string icon)
        {
            ModuleInfo m = new ModuleInfo { Key = key, Title = title, Icon = icon };
            Modules.Add(m);
            return m;
        }

        static void Form(ModuleInfo m, string key, string title, string legacy, string controller, string action, string icon)
        {
            FormInfo f = new FormInfo { Key = key, Title = title, LegacyTitle = legacy, Controller = controller, Action = action, Icon = icon, Module = m };
            m.Forms.Add(f);
            byKey[key] = f;
            byRoute[controller + "/" + action] = f;
        }

        public static IEnumerable<FormInfo> All { get { return Modules.SelectMany(m => m.Forms); } }

        public static FormInfo ByKey(string key)
        {
            FormInfo f;
            return key != null && byKey.TryGetValue(key, out f) ? f : null;
        }

        public static FormInfo ByRoute(string controller, string action)
        {
            FormInfo f;
            return byRoute.TryGetValue(controller + "/" + action, out f) ? f : null;
        }
    }
}
