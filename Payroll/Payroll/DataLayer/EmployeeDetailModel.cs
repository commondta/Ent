using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class EmployeeDetailModel
    {
        public int id { get; set; }
        public string PayrollName { get; set; }
        public string EmployeeNumber { get; set; }
        public string SalutationTitle { get; set; }
        public string LegalFirstName { get; set; }
        public string LegalLastName { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CompanyStartDate { get; set; }
        public string CitizenshipCountry { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string EmailAddress { get; set; }
        public string PostalAddress1 { get; set; }
        public string PostalAddress2 { get; set; }
        public string PostalAddress3 { get; set; }
        public string PostalTown { get; set; }
        public string PostalZipCode { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public string AccountNumber { get; set; }
        public string SwiftCode { get; set; }
        public string IBANno { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string BankPostalAddress1 { get; set; }
        public string BankPostalAddress2 { get; set; }
        public string DaysWorkedEachWeek { get; set; }
        public string HoursPerWeek { get; set; }
        public string CostCenter { get; set; }
        public string Department { get; set; }
        public DateTime PayrollAssignmentStartDate { get; set; }
        public DateTime PayrollAssignmentEndDate { get; set; }
        public string JobTitlePosition { get; set; }
        public string SalaryInstallments { get; set; }
        public string NationalIdentityCardNo { get; set; }
        public string CountryOfBirth { get; set; }
        public string NationalTaxNumber { get; set; }
        public string EmploymentContract { get; set; }
        public List<SalaryDetailModel> SalaryDetail { get; set; }
    }
}
