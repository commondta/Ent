using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BusinessLayer
{
    /// <summary>
    /// Idempotent schema upgrade for the admin database (Payroll_HCC):
    /// roles, permissions, extended accounts, activity log and approval queue.
    /// Runs on application start; every statement is guarded so re-running is harmless.
    /// A copy of the same DDL lives in "Payroll scripts\security_upgrade.sql" for DBAs.
    /// </summary>
    public static class SchemaUpgrade
    {
        public static readonly string[] Statements = new[]
        {
            @"IF OBJECT_ID('dbo.Role','U') IS NULL
              CREATE TABLE dbo.Role(
                  id INT IDENTITY(1,1) PRIMARY KEY,
                  Name VARCHAR(50) NOT NULL UNIQUE,
                  Description VARCHAR(200) NULL,
                  IsSystem BIT NOT NULL DEFAULT 0,
                  CreatedAt DATETIME NOT NULL DEFAULT GETDATE())",

            @"IF OBJECT_ID('dbo.RolePermission','U') IS NULL
              CREATE TABLE dbo.RolePermission(
                  id INT IDENTITY(1,1) PRIMARY KEY,
                  RoleId INT NOT NULL REFERENCES dbo.Role(id) ON DELETE CASCADE,
                  FormKey VARCHAR(100) NOT NULL,
                  CanView BIT NOT NULL DEFAULT 0,
                  CanCreate BIT NOT NULL DEFAULT 0,
                  CanEdit BIT NOT NULL DEFAULT 0,
                  CanDelete BIT NOT NULL DEFAULT 0,
                  CanApprove BIT NOT NULL DEFAULT 0,
                  CanExport BIT NOT NULL DEFAULT 0,
                  CanPrint BIT NOT NULL DEFAULT 0,
                  CONSTRAINT UQ_RolePermission UNIQUE(RoleId, FormKey))",
            "IF COL_LENGTH('dbo.RolePermission','CanExport') IS NULL ALTER TABLE dbo.RolePermission ADD CanExport BIT NOT NULL DEFAULT 0",
            "IF COL_LENGTH('dbo.RolePermission','CanPrint') IS NULL ALTER TABLE dbo.RolePermission ADD CanPrint BIT NOT NULL DEFAULT 0",

            "IF COL_LENGTH('dbo.Account','FullName') IS NULL ALTER TABLE dbo.Account ADD FullName VARCHAR(100) NULL",
            "IF COL_LENGTH('dbo.Account','Email') IS NULL ALTER TABLE dbo.Account ADD Email VARCHAR(150) NULL",
            "IF COL_LENGTH('dbo.Account','RoleId') IS NULL ALTER TABLE dbo.Account ADD RoleId INT NULL REFERENCES dbo.Role(id)",
            "IF COL_LENGTH('dbo.Account','IsActive') IS NULL ALTER TABLE dbo.Account ADD IsActive BIT NOT NULL DEFAULT 1",
            "IF COL_LENGTH('dbo.Account','MustChangePassword') IS NULL ALTER TABLE dbo.Account ADD MustChangePassword BIT NOT NULL DEFAULT 0",
            "IF COL_LENGTH('dbo.Account','LastLoginAt') IS NULL ALTER TABLE dbo.Account ADD LastLoginAt DATETIME NULL",
            "IF COL_LENGTH('dbo.Account','CreatedAt') IS NULL ALTER TABLE dbo.Account ADD CreatedAt DATETIME NOT NULL DEFAULT GETDATE()",
            "IF COL_LENGTH('dbo.Account','CreatedBy') IS NULL ALTER TABLE dbo.Account ADD CreatedBy VARCHAR(50) NULL",

            @"IF OBJECT_ID('dbo.ActivityLog','U') IS NULL
              CREATE TABLE dbo.ActivityLog(
                  id BIGINT IDENTITY(1,1) PRIMARY KEY,
                  OccurredAt DATETIME NOT NULL DEFAULT GETDATE(),
                  Username VARCHAR(50) NOT NULL,
                  Action VARCHAR(30) NOT NULL,
                  Module VARCHAR(60) NULL,
                  FormKey VARCHAR(100) NULL,
                  Detail VARCHAR(500) NULL,
                  IpAddress VARCHAR(50) NULL)",
            "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_ActivityLog_OccurredAt') CREATE INDEX IX_ActivityLog_OccurredAt ON dbo.ActivityLog(OccurredAt DESC)",

            @"IF OBJECT_ID('dbo.ApprovalRequest','U') IS NULL
              CREATE TABLE dbo.ApprovalRequest(
                  id INT IDENTITY(1,1) PRIMARY KEY,
                  RequestType VARCHAR(60) NOT NULL,
                  ReferenceKey VARCHAR(100) NULL,
                  Title VARCHAR(200) NOT NULL,
                  Detail VARCHAR(1000) NULL,
                  RequestedBy VARCHAR(50) NOT NULL,
                  RequestedAt DATETIME NOT NULL DEFAULT GETDATE(),
                  Status VARCHAR(20) NOT NULL DEFAULT 'Pending',
                  ReviewedBy VARCHAR(50) NULL,
                  ReviewedAt DATETIME NULL,
                  ReviewComment VARCHAR(500) NULL)",

            @"IF OBJECT_ID('dbo.ApprovalHistory','U') IS NULL
              CREATE TABLE dbo.ApprovalHistory(
                  id INT IDENTITY(1,1) PRIMARY KEY,
                  RequestId INT NOT NULL REFERENCES dbo.ApprovalRequest(id) ON DELETE CASCADE,
                  Action VARCHAR(20) NOT NULL,
                  ActionBy VARCHAR(50) NOT NULL,
                  ActionAt DATETIME NOT NULL DEFAULT GETDATE(),
                  Comment VARCHAR(500) NULL)",

            // Approval processes (Approval Setup): switch per request type, ordered stages, approver role / named users
            "IF COL_LENGTH('dbo.ApprovalRequest','CurrentStage') IS NULL ALTER TABLE dbo.ApprovalRequest ADD CurrentStage INT NOT NULL DEFAULT 1",
            "IF COL_LENGTH('dbo.ApprovalHistory','Stage') IS NULL ALTER TABLE dbo.ApprovalHistory ADD Stage INT NULL",
            @"IF OBJECT_ID('dbo.ApprovalProcess','U') IS NULL
              CREATE TABLE dbo.ApprovalProcess(
                  RequestType VARCHAR(60) NOT NULL PRIMARY KEY,
                  Title VARCHAR(100) NOT NULL,
                  Description VARCHAR(300) NULL,
                  IsEnabled BIT NOT NULL DEFAULT 1,
                  UpdatedBy VARCHAR(50) NULL,
                  UpdatedAt DATETIME NULL)",
            @"IF OBJECT_ID('dbo.ApprovalStage','U') IS NULL
              CREATE TABLE dbo.ApprovalStage(
                  id INT IDENTITY(1,1) PRIMARY KEY,
                  RequestType VARCHAR(60) NOT NULL REFERENCES dbo.ApprovalProcess(RequestType) ON DELETE CASCADE,
                  StageNo INT NOT NULL,
                  Name VARCHAR(100) NOT NULL,
                  ApproverRoleId INT NULL,
                  RequiredCount INT NOT NULL DEFAULT 1)",
            @"IF OBJECT_ID('dbo.ApprovalStageUser','U') IS NULL
              CREATE TABLE dbo.ApprovalStageUser(
                  StageId INT NOT NULL REFERENCES dbo.ApprovalStage(id) ON DELETE CASCADE,
                  UserId INT NOT NULL,
                  PRIMARY KEY (StageId, UserId))",
            "IF NOT EXISTS (SELECT 1 FROM dbo.ApprovalProcess WHERE RequestType='UserAccount') INSERT INTO dbo.ApprovalProcess(RequestType,Title,Description) VALUES('UserAccount','New user account','An account created by someone without the Approve right on User Management stays inactive until this request is approved.')",
            "IF NOT EXISTS (SELECT 1 FROM dbo.ApprovalProcess WHERE RequestType='PayrollRun') INSERT INTO dbo.ApprovalProcess(RequestType,Title,Description) VALUES('PayrollRun','Payroll processing run','Every payroll run is queued for review after processing; the run itself is kept and the queue records its approval.')",
            "INSERT INTO dbo.ApprovalStage(RequestType,StageNo,Name) SELECT p.RequestType,1,'Approval' FROM dbo.ApprovalProcess p WHERE NOT EXISTS (SELECT 1 FROM dbo.ApprovalStage s WHERE s.RequestType=p.RequestType)",
            // Form gate: any registered form can require approval for Create / Edit / Delete; the write request is held and replayed
            "IF COL_LENGTH('dbo.ApprovalProcess','FormKey') IS NULL ALTER TABLE dbo.ApprovalProcess ADD FormKey VARCHAR(100) NULL",
            "IF COL_LENGTH('dbo.ApprovalProcess','Actions') IS NULL ALTER TABLE dbo.ApprovalProcess ADD Actions VARCHAR(50) NULL",
            "IF COL_LENGTH('dbo.ApprovalRequest','Payload') IS NULL ALTER TABLE dbo.ApprovalRequest ADD Payload NVARCHAR(MAX) NULL",
            "IF COL_LENGTH('dbo.ApprovalRequest','AppliedAt') IS NULL ALTER TABLE dbo.ApprovalRequest ADD AppliedAt DATETIME NULL",
            "IF COL_LENGTH('dbo.ApprovalRequest','AppliedBy') IS NULL ALTER TABLE dbo.ApprovalRequest ADD AppliedBy VARCHAR(50) NULL",

            // Seed roles
            "IF NOT EXISTS (SELECT 1 FROM dbo.Role WHERE Name='Administrator') INSERT INTO dbo.Role(Name,Description,IsSystem) VALUES('Administrator','Full access to every module, user management and approvals.',1)",
            "IF NOT EXISTS (SELECT 1 FROM dbo.Role WHERE Name='Payroll Officer') INSERT INTO dbo.Role(Name,Description,IsSystem) VALUES('Payroll Officer','Runs payroll, attendance and transactions; cannot manage users.',0)",
            "IF NOT EXISTS (SELECT 1 FROM dbo.Role WHERE Name='HR Officer') INSERT INTO dbo.Role(Name,Description,IsSystem) VALUES('HR Officer','Maintains employees, departments, leave and loan requests.',0)",
            "IF NOT EXISTS (SELECT 1 FROM dbo.Role WHERE Name='Viewer') INSERT INTO dbo.Role(Name,Description,IsSystem) VALUES('Viewer','Read-only access to reports and master data.',0)",

            // Accounts that pre-date roles become administrators (there was a single admin login before this upgrade)
            "UPDATE dbo.Account SET RoleId=(SELECT id FROM dbo.Role WHERE Name='Administrator') WHERE RoleId IS NULL",
            "UPDATE dbo.Account SET FullName='System Administrator' WHERE Username='admin' AND FullName IS NULL",
        };

        /// <summary>Default form access for the seeded non-system roles (applied only when a role has no permission rows yet).</summary>
        public static readonly Dictionary<string, string[]> DefaultRoleForms = new Dictionary<string, string[]>
        {
            { "Payroll Officer", new[] { "Home.Dashboard", "Setup.PayElements", "Setup.PayrollPeriods", "Setup.BonusSetup", "Setup.PayrollFormulas", "Setup.PayrollTaxRules", "Setup.EOBISetup",
                                          "Attendance.Daily", "Attendance.Monthly", "Payroll.MonthlyAdditions", "Payroll.MonthlyDeductions", "Payroll.PaymentsDeductions", "Payroll.Processing",
                                          "Overtime.Processing", "Overtime.Formulas", "Reports.Personal", "Reports.Address", "Reports.Bank", "Reports.Job", "Security.Approvals", "Security.ActivityLog" } },
            { "HR Officer", new[] { "Home.Dashboard", "Employees.Companies", "Employees.Profiles", "Employees.Categories", "Employees.Departments", "Benefits.LeaveTypes", "Benefits.LoanSetup", "Benefits.OvertimeRules",
                                     "Leave.Requests", "Leave.Settlement", "Loans.Requests", "Reports.Personal", "Reports.Address", "Reports.Bank", "Reports.Job", "Security.Approvals" } },
            { "Viewer", new[] { "Home.Dashboard", "Employees.Profiles", "Employees.Departments", "Reports.Personal", "Reports.Address", "Reports.Bank", "Reports.Job" } },
        };

        public static void Apply(string connectionString)
        {
            Database db = new Database(connectionString);
            foreach (string sql in Statements)
                db.Set(sql);

            foreach (KeyValuePair<string, string[]> kv in DefaultRoleForms)
            {
                var count = db.Get("SELECT COUNT(*) FROM RolePermission rp JOIN Role r ON r.id=rp.RoleId WHERE r.Name=@n", new SqlParameter("@n", kv.Key));
                if (Convert.ToInt32(count.Rows[0][0]) > 0) continue;

                bool fullAccess = kv.Key != "Viewer";
                foreach (string form in kv.Value)
                {
                    bool canApprove = fullAccess && form == "Security.Approvals";
                    db.Set(@"INSERT INTO RolePermission(RoleId,FormKey,CanView,CanCreate,CanEdit,CanDelete,CanApprove,CanExport,CanPrint)
                             SELECT id,@f,1,@c,@c,@d,@a,1,1 FROM Role WHERE Name=@n",
                        new SqlParameter("@f", form),
                        new SqlParameter("@c", fullAccess),
                        new SqlParameter("@d", fullAccess && !form.StartsWith("Security.")),
                        new SqlParameter("@a", canApprove),
                        new SqlParameter("@n", kv.Key));
                }
            }
        }
    }
}
