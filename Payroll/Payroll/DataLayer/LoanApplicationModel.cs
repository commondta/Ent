using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    class LoanApplicationModel
    {
        public int id { get; set; }
        public string EmployeeType { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string LoanCode { get; set; }
        public string LoanType { get; set; }
        public string LoanAmount { get; set; }
        public string SanctionedAmount { get; set; }
        public string RateofInterest { get; set; }
        public string NoofInstallments { get; set; }
        public string AmountorMonth { get; set; }
        public string InterestAmount { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string Status { get; set; }
        public string PetronID { get; set; }
        public string EffectivePayPeriod { get; set; }
        public string EffectiveDate { get; set; }
        public string DeductedAmount { get; set; }
        public string PendingAmount { get; set; }
        public string PreviousLoanAmount { get; set; }
        public string PreviousLoanPendingAmount { get; set; }
        public string Remarks { get; set; }
    }
}
