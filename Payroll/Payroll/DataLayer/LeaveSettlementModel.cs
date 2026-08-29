using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll_HCC.Models
{
    public class LeaveSettlementModel
    {
        public int id { get; set; }
        public string Type { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string CompEmpID { get; set; }
        public string LeaveApplNumber { get; set; }
        public string LeaveAmount { get; set; }
        public string CurrentPayPeriod { get; set; }
        public string OtherAmount { get; set; }
        public string TotalAmount { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string ApprovedFromDate { get; set; }
        public string ApprovedtoDate { get; set; }
        public string ApprovedDays { get; set; }
        public string EligibleDays { get; set; }
        public string PreviousPayPeriod { get; set; }
    }
}