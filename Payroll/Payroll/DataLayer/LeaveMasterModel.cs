using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll_HCC.Models
{
    public class LeaveMasterModel
    {
        public int id { get; set; }
        public string LeaveCode { get; set; }
        public string MaxMonthlyAplli { get; set; }
        public string Description { get; set; }
        public string TotalLeavesinYear { get; set; }
        public string TotalLeavesinYearForTrainer { get; set; }
        public string MinContinuous { get; set; }
        public string MaxContinuous { get; set; }
        public string MinContiDurProb { get; set; }
        public string MaxContiDurProb { get; set; }
        public string LeaveType { get; set; }
        public string ApplicableDuringProbation { get; set; }
        public string Encashable { get; set; }
        public string EffectiveFrom { get; set; }
        public string CarryForwardtoNextYear { get; set; }
        public string MinBalanceForEncash { get; set; }
        public string MaxLeaveCarryForward { get; set; }
        public string MaxLeavetoEncash { get; set; }
        public string CompanyLeavePolicy { get; set; }
        public string PayableLaeave { get; set; }
        public string Closee { get; set; }
        public string Remarks { get; set; }
    }
}