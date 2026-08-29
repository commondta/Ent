using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll_HCC.Models
{
    public class LeaveApplicationModel
    {
        public int id { get; set; }
        public string Location { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Nationality { get; set; }
        public string PassportNo { get; set; }
        public string LastLeavefromDate { get; set; }
        public string LastLeavetoDate { get; set; }
        public string LeaveCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string NoofDaysLeaveRequired { get; set; }
        public string BalanceLeave { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentDate { get; set; }
        public string Status { get; set; }
        public string DOJ { get; set; }
        public string DOJafterLeave { get; set; }
        public string LeaveType { get; set; }
        public string Signedby { get; set; }
        public string LeaveAddress { get; set; }
        public string ContactNo { get; set; }
        public string Preparedby { get; set; }
        public string Notes { get; set; }
        public string Recommendedby { get; set; }
        public string LeaveRecommendedfrom { get; set; }
        public string LeaveRecommendedto { get; set; }
        public string NoofDaysRecommended { get; set; }
        public string ApprovedbyDOorGM { get; set; }
        public string LeaveApprovedFrom { get; set; }
        public string LeaveApprovedto { get; set; }
        public string NoofDaysApproved { get; set; }
        public string RejoiningDate { get; set; }
        public string EarnedLeaveDue { get; set; }
        public string Approved { get; set; }
        public string Settlementfor { get; set; }
    }
}