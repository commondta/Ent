using Payroll_HCC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace BusinessLayer
{
    public class Company
    {
        Database database;
        string query;

        public Company(string connectionString)
        {
            database = new Database(connectionString);
        }

        public void Insert(CompanyModel obj)
        {
            query = "INSERT INTO Company (CompanyName, TaxNo, ESINo, AnnualDays, Address, Active) VALUES (" +
                                                "@CompanyName, " +
                                                "@TaxNo, " +
                                                "@ESINo, " +
                                                "@AnnualDays, " +
                                                "@Address, 'False'); " +
                    "SELECT IDENT_CURRENT( 'Company' )";
            DataTable dt = database.Get(query,
                new SqlParameter("@CompanyName", (object)obj.CompanyName ?? DBNull.Value),
                new SqlParameter("@TaxNo", (object)obj.TaxNo ?? DBNull.Value),
                new SqlParameter("@ESINo", (object)obj.ESINo ?? DBNull.Value),
                new SqlParameter("@AnnualDays", (object)obj.AnnualDays ?? DBNull.Value),
                new SqlParameter("@Address", (object)obj.Address ?? DBNull.Value));
            long newCompanyId = long.Parse(dt.Rows[0][0].ToString()); // identifier, sanitized
            CreateDatabase("[Payroll_Company" + newCompanyId + "]");
        }

        public void Delete(string id)
        {
            int companyId = int.Parse(id); // identifier, sanitized
            query = "DELETE FROM Company WHERE id = @id;" +
                    "DROP DATABASE Payroll_Company" + companyId;
            database.Set(query, new SqlParameter("@id", companyId));
        }

        public List<CompanyModel> getAll()
        {
            query = "SELECT * FROM Company";
            DataTable dt = database.Get(query);

            return dataTableToList(dt);
        }

        public void setActive()
        {
            query = "SELECT id FROM Company WHERE Active='True'";
            DataTable dt = database.Get(query);
            if (dt.Rows.Count != 0)
            {
                SetDatabase(dt.Rows[0][0].ToString());
            }
        }

        public string getActiveName()
        {
            query = "SELECT CompanyName FROM Company WHERE Active='True'";
            DataTable dt = database.Get(query);
            string activeCompany = "";

            if (dt.Rows.Count != 0)
            {
                activeCompany = dt.Rows[0][0].ToString();
            }

            return activeCompany;
        }

        private void SetDatabase(string id)
        {
            var configuration = WebConfigurationManager.OpenWebConfiguration("~");
            var section = (ConnectionStringsSection)configuration.GetSection("connectionStrings");
            int companyId = int.Parse(id); // identifier, sanitized
            // local dev: the original hardcoded B1DEVSERVER\SA (sa/B1admin) does not exist on this PC
            string newConnectionString = "data source=.\\MSSQLSERVER01;initial catalog=Payroll_Company" + companyId + ";Integrated Security=True";

            // Only write Web.config when the active company actually changed.
            // Saving unconditionally rewrote Web.config on EVERY request (setActive runs
            // per-request), which recycled the app domain and wiped all sessions -
            // making it impossible to stay logged in.
            string current = section.ConnectionStrings["DefaultConnection"].ConnectionString;
            if (string.Equals(current, newConnectionString, StringComparison.OrdinalIgnoreCase))
                return;

            section.ConnectionStrings["DefaultConnection"].ConnectionString = newConnectionString;
            configuration.Save();
        }

        public void Switch(string id)
        {
            query = "UPDATE Company SET Active='False' WHERE Active='True'; " +
                    "UPDATE Company SET Active='True' WHERE id=@id; ";
            DataTable dt = database.Get(query, new SqlParameter("@id", (object)id ?? DBNull.Value));
            if (dt.Rows.Count != 0)
            {
                SetDatabase(dt.Rows[0][0].ToString());
            }
        }

        private List<CompanyModel> dataTableToList(DataTable dt)
        {
            List<CompanyModel> list = new List<CompanyModel>();
            CompanyModel obj;
            foreach (DataRow row in dt.Rows)
            {
                obj = new CompanyModel();
                obj.id = Convert.ToInt32(row[0]);
                obj.CompanyName = row[1].ToString();
                obj.TaxNo = row[2].ToString();
                obj.ESINo = row[3].ToString();
                obj.AnnualDays = row[4].ToString();
                obj.Address = row[5].ToString();

                list.Add(obj);
            }

            return list;
        }

        public void CreateDatabase(string companyName)
        {
            // identifier, sanitized: only "[Payroll_Company<number>]" (brackets optional) is accepted;
            // the numeric suffix is coerced via long.Parse and the name is rebuilt from the fixed prefix.
            string trimmedName = companyName.Trim().TrimStart('[').TrimEnd(']');
            const string prefix = "Payroll_Company";
            if (!trimmedName.StartsWith(prefix, StringComparison.Ordinal))
            {
                throw new ArgumentException("Invalid company database name.", "companyName");
            }
            long companyId = long.Parse(trimmedName.Substring(prefix.Length));
            string safeCompanyName = "[" + prefix + companyId + "]";

            query = "CREATE DATABASE " + safeCompanyName;
            database.Set(query);

            query = "USE " + safeCompanyName + " ";

            query += @"CREATE TABLE [dbo].[CompanySetup](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [CompanyName] [varchar](50) NOT NULL,
	                    [TaxNo] [nvarchar](50) NOT NULL,
	                    [ESINo] [nvarchar](50) NOT NULL,
	                    [AnnualDays] [int] NOT NULL,
	                    [Address] [nvarchar](250) NOT NULL,
                     CONSTRAINT [PK_CompanySetup] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[DepartmentSetup](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [DepartmentName] [varchar](50) NULL,
	                    [Description] [varchar](50) NULL,
                     CONSTRAINT [PK_DepartmentSetup] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[EmployeeCategoryMaster](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [EmployeeCategoryCode] [nvarchar](50) NOT NULL,
	                    [EmployeeCategoryName] [varchar](50) NOT NULL,
	                    [AccountCode] [nvarchar](50) NOT NULL,
	                    [Remarks] [varchar](250) NOT NULL,
                     CONSTRAINT [PK_EmployeeMaster] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[EmployeeDetail](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [PayrollName] [varchar](50) NULL,
	                    [EmployeeNumber] [varchar](50) NULL,
	                    [SalutationTitle] [varchar](50) NULL,
	                    [LegalFirstName] [varchar](50) NULL,
	                    [LegalLastName] [varchar](50) NULL,
	                    [MaritalStatus] [varchar](15) NULL,
	                    [Gender] [varchar](10) NULL,
	                    [DateOfBirth] [datetime] NULL,
	                    [CompanyStartDate] [datetime] NULL,
	                    [CitizenshipCountry] [varchar](50) NULL,
	                    [PhoneNo] [varchar](50) NULL,
	                    [MobileNo] [varchar](50) NULL,
	                    [EmailAddress] [varchar](50) NULL,
	                    [PostalAddress1] [varchar](100) NULL,
	                    [PostalAddress2] [varchar](100) NULL,
	                    [PostalAddress3] [varchar](100) NULL,
	                    [PostalTown] [varchar](50) NULL,
	                    [PostalZipCode] [varchar](50) NULL,
	                    [AccountName] [varchar](50) NULL,
	                    [AccountType] [varchar](50) NULL,
	                    [AccountNumber] [varchar](50) NULL,
	                    [SwiftCode] [varchar](50) NULL,
	                    [IBANno] [varchar](50) NULL,
	                    [BankName] [varchar](50) NULL,
	                    [BranchName] [varchar](50) NULL,
	                    [BranchCode] [varchar](50) NULL,
	                    [BankPostalAddress1] [varchar](100) NULL,
	                    [BankPostalAddress2] [varchar](100) NULL,
	                    [DaysWorkedEachWeek] [varchar](50) NULL,
	                    [HoursPerWeek] [varchar](50) NULL,
	                    [CostCenter] [varchar](50) NULL,
	                    [Department] [varchar](50) NULL,
	                    [PayrollAssignmentStartDate] [datetime] NULL,
	                    [PayrollAssignmentEndDate] [datetime] NULL,
	                    [JobTitlePosition] [varchar](50) NULL,
	                    [SalaryInstallments] [varchar](50) NULL,
	                    [NationalIdentityCardNo] [varchar](50) NULL,
	                    [CountryOfBirth] [varchar](50) NULL,
	                    [NationalTaxNumber] [varchar](50) NULL,
	                    [EmploymentContract] [varchar](50) NULL,
                     CONSTRAINT [PK_EmployeeDetail] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[FormulaMasterChild](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [ParentID] [int] NOT NULL,
	                    [PayCode] [nvarchar](50) NOT NULL,
	                    [AmtHigherLimit] [nvarchar](150) NOT NULL,
	                    [AmtLowerLimit] [nvarchar](150) NOT NULL,
	                    [Percentages] [float] NOT NULL,
	                    [Remarks] [nvarchar](150) NOT NULL,
                     CONSTRAINT [PK_FormulaMaster2] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[FormulaMasterParent](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [EmployeeCategory] [varchar](50) NOT NULL,
	                    [Name] [varchar](50) NOT NULL,
                     CONSTRAINT [PK_FormulaMaster] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[LeaveApplication](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [Location] [varchar](150) NOT NULL,
	                    [EmployeeID] [int] NOT NULL,
	                    [EmployeeName] [varchar](150) NOT NULL,
	                    [Designation] [varchar](150) NOT NULL,
	                    [Nationality] [varchar](150) NOT NULL,
	                    [PassportNo] [nvarchar](150) NOT NULL,
	                    [LastLeavefromDate] [date] NOT NULL,
	                    [LastLeavetoDate] [date] NOT NULL,
	                    [LeaveCode] [nvarchar](50) NOT NULL,
	                    [FromDate] [date] NOT NULL,
	                    [ToDate] [date] NOT NULL,
	                    [NoofDaysLeaveRequired] [int] NOT NULL,
	                    [BalanceLeave] [float] NOT NULL,
	                    [DocumentNo] [nvarchar](50) NOT NULL,
	                    [DocumentDate] [date] NOT NULL,
	                    [Status] [varchar](50) NOT NULL,
	                    [DOJ] [varchar](150) NOT NULL,
	                    [DOJafterLeave] [nvarchar](150) NOT NULL,
	                    [LeaveType] [nvarchar](50) NOT NULL,
	                    [Signedby] [varchar](150) NOT NULL,
	                    [LeaveAddress] [nvarchar](150) NOT NULL,
	                    [ContactNo] [int] NOT NULL,
	                    [Preparedby] [varchar](150) NOT NULL,
	                    [Notes] [varchar](250) NOT NULL,
	                    [Recommendedby] [varchar](150) NOT NULL,
	                    [LeaveRecommendedfrom] [varchar](150) NOT NULL,
	                    [LeaveRecommendedto] [varchar](150) NOT NULL,
	                    [NoofDaysRecommended] [int] NOT NULL,
	                    [ApprovedbyDOorGM] [varchar](150) NOT NULL,
	                    [LeaveApprovedFrom] [nvarchar](50) NOT NULL,
	                    [LeaveApprovedto] [nvarchar](50) NOT NULL,
	                    [NoofDaysApproved] [nvarchar](50) NOT NULL,
	                    [RejoiningDate] [date] NOT NULL,
	                    [EarnedLeaveDue] [nvarchar](50) NOT NULL,
	                    [Approved] [varchar](50) NOT NULL,
	                    [Settlementfor] [nvarchar](50) NOT NULL,
                     CONSTRAINT [PK_LeaveApplication] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[LeaveDetail](
	                    [EmployeeDetailKey] [int] NOT NULL,
	                    [LeaveCode] [varchar](50) NULL,
	                    [LeaveName] [varchar](50) NULL,
	                    [LeaveType] [varchar](50) NULL,
	                    [TotalLeavesInYear] [numeric](5, 0) NULL,
	                    [BalLeaveLastYear] [numeric](10, 0) NULL,
	                    [MinBalanceForEncashment] [numeric](18, 0) NULL,
	                    [MaxLeaveToEncashment] [numeric](18, 0) NULL,
	                    [CarryForwardToNextYear] [bit] NULL,
	                    [MaxMonthlyApplicable] [varchar](50) NULL,
	                    [MinContinuous] [varchar](50) NULL,
	                    [MaxContinuous] [varchar](50) NULL,
	                    [Encashable] [bit] NULL,
	                    [EffectiveFrom] [date] NULL,
	                    [MaxLeavesToForward] [numeric](18, 0) NULL,
	                    [AlreadyTaken] [varchar](50) NULL,
	                    [Balance] [varchar](50) NULL
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[LeaveMaster](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [LeaveCode] [int] NOT NULL,
	                    [MaxMonthlyAplli] [nvarchar](50) NOT NULL,
	                    [Description] [varchar](350) NOT NULL,
	                    [TotalLeavesinYear] [int] NOT NULL,
	                    [TotalLeavesinYearForTrainer] [int] NOT NULL,
	                    [MinContinuous] [nvarchar](150) NOT NULL,
	                    [MaxContinuous] [nvarchar](150) NOT NULL,
	                    [MinContiDurProb] [nvarchar](50) NOT NULL,
	                    [MaxContiDurProb] [nvarchar](50) NOT NULL,
	                    [LeaveType] [varchar](50) NOT NULL,
	                    [ApplicableDuringProbation] [varchar](50) NOT NULL,
	                    [Encashable] [varchar](50) NOT NULL,
	                    [EffectiveFrom] [varchar](150) NOT NULL,
	                    [CarryForwardtoNextYear] [varchar](50) NOT NULL,
	                    [MinBalanceForEncash] [float] NOT NULL,
	                    [MaxLeaveCarryForward] [int] NOT NULL,
	                    [MaxLeavetoEncash] [int] NOT NULL,
	                    [CompanyLeavePolicy] [varchar](350) NOT NULL,
	                    [PayableLaeave] [int] NOT NULL,
	                    [Closee] [varchar](50) NOT NULL,
	                    [Remarks] [varchar](450) NOT NULL,
                     CONSTRAINT [PK_LeaveMaster] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[LeaveMaster2](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [tid] [int] NOT NULL,
	                    [FromYear] [date] NOT NULL,
	                    [ToYear] [date] NOT NULL,
	                    [Closee] [varchar](50) NOT NULL,
                     CONSTRAINT [PK_LeaveMaster2] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[LeaveSettlement](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [Type] [varchar](50) NOT NULL,
	                    [EmployeeID] [int] NOT NULL,
	                    [EmployeeName] [varchar](150) NOT NULL,
	                    [CompEmpID] [nvarchar](50) NOT NULL,
	                    [LeaveApplNumber] [nvarchar](50) NOT NULL,
	                    [LeaveAmount] [float] NOT NULL,
	                    [CurrentPayPeriod] [nvarchar](50) NOT NULL,
	                    [OtherAmount] [float] NOT NULL,
	                    [TotalAmount] [float] NOT NULL,
	                    [DocumentNo] [nvarchar](50) NOT NULL,
	                    [DocumentDate] [date] NOT NULL,
	                    [ApprovedFromDate] [date] NOT NULL,
	                    [ApprovedtoDate] [date] NOT NULL,
	                    [ApprovedDays] [nvarchar](50) NOT NULL,
	                    [EligibleDays] [nvarchar](50) NOT NULL,
	                    [PreviousPayPeriod] [nvarchar](50) NOT NULL,
                     CONSTRAINT [PK_LeaveSettlement] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[LoanApplication](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [EmployeeType] [varchar](50) NOT NULL,
	                    [EmployeeID] [int] NOT NULL,
	                    [EmployeeName] [varchar](50) NOT NULL,
	                    [Designation] [nvarchar](150) NOT NULL,
	                    [LoanCode] [nvarchar](50) NOT NULL,
	                    [LoanType] [varchar](50) NOT NULL,
	                    [LoanAmount] [float] NOT NULL,
	                    [SanctionedAmount] [float] NOT NULL,
	                    [RateofInterest] [float] NOT NULL,
	                    [NoofInstallments] [int] NOT NULL,
	                    [AmountorMonth] [nvarchar](50) NOT NULL,
	                    [InterestAmount] [float] NOT NULL,
	                    [DocumentNo] [nvarchar](50) NOT NULL,
	                    [DocumentDate] [date] NOT NULL,
	                    [Status] [varchar](50) NOT NULL,
	                    [PetronID] [varchar](50) NOT NULL,
	                    [EffectivePayPeriod] [nvarchar](50) NOT NULL,
	                    [EffectiveDate] [date] NOT NULL,
	                    [DeductedAmount] [float] NOT NULL,
	                    [PendingAmount] [float] NOT NULL,
	                    [PreviousLoanAmount] [float] NOT NULL,
	                    [PreviousLoanPendingAmount] [float] NOT NULL,
	                    [Remarks] [varchar](450) NOT NULL,
                     CONSTRAINT [PK_LoanApplication] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[LoanApplication2](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [tid] [int] NOT NULL,
	                    [Month] [varchar](50) NOT NULL,
	                    [Year] [varchar](50) NOT NULL,
	                    [Date] [date] NOT NULL,
	                    [Amount] [float] NOT NULL,
	                    [Status] [varchar](50) NOT NULL,
                     CONSTRAINT [PK_LoanApplication2] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[LoanMaster](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [Code] [nvarchar](150) NOT NULL,
	                    [Description] [varchar](350) NOT NULL,
	                    [LoanType] [varchar](150) NOT NULL,
	                    [MaxAmount] [varchar](150) NOT NULL,
	                    [RateofInterest] [varchar](150) NOT NULL,
	                    [MinRepaymentAmount] [float] NOT NULL,
	                    [MaxNoofInstallments] [float] NOT NULL,
	                    [Remarks] [varchar](350) NOT NULL,
                     CONSTRAINT [PK_LoanMaster] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[OverTimeMaster](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [Code] [nvarchar](100) NOT NULL,
	                    [Type] [varchar](100) NOT NULL,
	                    [MaxOTHours] [int] NOT NULL,
	                    [MinOTHours] [int] NOT NULL,
	                    [Factors] [float] NOT NULL,
                     CONSTRAINT [PK_OverTimeMaster] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[OvertimeProcess](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [PayPeriod] [nvarchar](50) NOT NULL,
	                    [PayMonth] [varchar](50) NOT NULL,
	                    [FromDate] [date] NOT NULL,
	                    [ToDate] [date] NOT NULL,
	                    [DocumentNo] [nvarchar](150) NOT NULL,
	                    [DocumentDate] [date] NOT NULL,
	                    [Status] [varchar](150) NOT NULL,
                     CONSTRAINT [PK_OvertimeProcess] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[OvertimeProcess2](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [tid] [int] NOT NULL,
	                    [EmployeeID] [int] NOT NULL,
	                    [Name] [varchar](150) NOT NULL,
	                    [Designation] [varchar](150) NOT NULL,
	                    [Department] [varchar](150) NOT NULL,
	                    [Location] [varchar](150) NOT NULL,
	                    [CompanyID] [int] NOT NULL,
	                    [NetAmount] [float] NOT NULL,
	                    [Basic] [varchar](150) NOT NULL,
	                    [WeekDaysOTHours] [nvarchar](150) NOT NULL,
	                    [WeekDaysOTAmount] [nvarchar](150) NOT NULL,
	                    [WeekEndOTHours] [nvarchar](50) NOT NULL,
	                    [WeekEndOTAmount] [nvarchar](50) NOT NULL,
	                    [MIGAllowance] [nvarchar](50) NOT NULL,
	                    [GrossSalary] [float] NOT NULL,
	                    [OtherDeductions] [float] NOT NULL,
	                    [Remarks] [varchar](350) NOT NULL,
                     CONSTRAINT [PK_OvertimeProcess2] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[PayElements](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [PayElementCode] [varchar](150) NULL,
	                    [Description] [varchar](250) NULL,
	                    [Type] [varchar](150) NULL,
	                    [PayElementType] [varchar](150) NULL,
	                    [Amount] [int] NULL,
	                    [EffectiveDate] [datetime] NULL,
	                    [Taxable] [varchar](150) NULL,
                     CONSTRAINT [PK_PayElements] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[PaymentsAndDeductionsChild](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [ParentID] [int] NOT NULL,
	                    [PayrollName] [varchar](150) NOT NULL,
	                    [EmployeeName] [varchar](150) NOT NULL,
	                    [EmployeeID] [int] NOT NULL,
	                    [PayrollPayElement] [varchar](50) NOT NULL,
	                    [TransactionType] [varchar](50) NOT NULL,
	                    [EffectiveDate] [date] NOT NULL,
	                    [EndDate] [date] NOT NULL,
	                    [Recurrence] [nvarchar](250) NOT NULL,
	                    [Amount] [float] NOT NULL,
	                    [Currency] [varchar](50) NOT NULL,
	                    [Comments] [varchar](250) NOT NULL,
                     CONSTRAINT [PK_PaymentsAndDeductions2] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[PaymentsAndDeductionsParent](
	                    [DocumentNo] [int] IDENTITY(1,1) NOT NULL,
	                    [PayPeriod] [varchar](150) NOT NULL,
	                    [DocumentDate] [date] NOT NULL,
	                    [Status] [varchar](50) NOT NULL,
                     CONSTRAINT [PK_PaymentsAndDeductions] PRIMARY KEY CLUSTERED 
                    (
	                    [DocumentNo] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[PayPeriod](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [LocationProjectSite] [nvarchar](150) NOT NULL,
	                    [PayPeriodCodeMonth] [nvarchar](50) NOT NULL,
	                    [Name] [varchar](150) NOT NULL,
	                    [FromDate] [datetime] NOT NULL,
	                    [ToDate] [datetime] NOT NULL,
	                    [PayMonth] [varchar](50) NOT NULL,
	                    [NoOfWorkingDays] [int] NOT NULL,
	                    [NoOfFridays] [int] NOT NULL,
	                    [NoOfHolidays] [int] NOT NULL,
	                    [MaximumNormalOTHoursMonth] [int] NOT NULL,
	                    [MaximumWorkingHoursMonth] [int] NOT NULL,
	                    [Remarks] [varchar](50) NOT NULL,
                     CONSTRAINT [PK_PayPeriod] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[PayrollProcessChild](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [ParentID] [int] NOT NULL,
	                    [EmployeeID] [int] NOT NULL,
	                    [Name] [varchar](150) NOT NULL,
	                    [IncomeTax] [float] NOT NULL,
	                    [TotalDeduction] [float] NOT NULL,
	                    [NetSalary] [float] NOT NULL,
	                    [TaxableSalary] [float] NULL,
                     CONSTRAINT [PK_PayrollProcess2] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[PayrollProcessParent](
	                    [EmployeeType] [varchar](150) NOT NULL,
	                    [PayPeriod] [varchar](150) NOT NULL,
	                    [PayMonth] [varchar](50) NOT NULL,
	                    [FromDate] [date] NOT NULL,
	                    [ToDate] [date] NOT NULL,
	                    [DocumentNo] [int] IDENTITY(1,1) NOT NULL,
	                    [DocumentDate] [date] NOT NULL,
	                    [Status] [varchar](150) NOT NULL,
	                    [PostJE] [varchar](150) NOT NULL,
	                    [PostingDate] [date] NOT NULL,
                     CONSTRAINT [PK_PayrollProcessParent] PRIMARY KEY CLUSTERED 
                    (
	                    [DocumentNo] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[SalaryDetail](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [employeeId] [int] NOT NULL,
	                    [Code] [varchar](50) NULL,
	                    [Name] [varchar](50) NULL,
	                    [EffectiveDate] [datetime] NULL,
	                    [Type] [varchar](50) NULL,
	                    [Amount] [varchar](50) NULL,
	                    [OT] [bit] NULL,
	                    [Tax] [bit] NULL
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[TaxFormulaCalculationChild](
	                    [id] [int] IDENTITY(1,1) NOT NULL,
	                    [ParentId] [int] NOT NULL,
	                    [LowerAmount] [int] NOT NULL,
	                    [HigherAmount] [int] NOT NULL,
	                    [Percentage] [float] NOT NULL,
	                    [FixedAmount] [int] NOT NULL,
	                    [OtherAmount] [int] NOT NULL,
	                    [Remarks] [varchar](350) NOT NULL,
                     CONSTRAINT [PK_TaxFormulaCalculation2] PRIMARY KEY CLUSTERED 
                    (
	                    [id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]

                    CREATE TABLE [dbo].[TaxFormulaCalculationParent](
	                    [Code] [int] IDENTITY(1,1) NOT NULL,
	                    [FromDate] [date] NOT NULL,
	                    [ToDate] [date] NOT NULL,
	                    [DocumentDate] [date] NOT NULL,
                     CONSTRAINT [PK_TaxFormulaCalculationParent] PRIMARY KEY CLUSTERED 
                    (
	                    [Code] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY]
                    ";

            database.Set(query);
        }
    }
}
