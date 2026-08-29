using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class LeaveDetailModel
    {
        public int leaveCode { get; set; }
        public string leaveName { get; set; }
        public string leaveType { get; set; }
        public string totalLeavesInYear { get; set; }
        public string BalLeaveLastYear { get; set; }
        public string MinBalanceForEncashment { get; set; }
        public string MaxLeaveToEncashment { get; set; }
        public string CarryForwardToNextYear { get; set; }
        public string MaxMonthlyApplicable { get; set; }
        public string MinContinuous { get; set; }
        public string MaxContinuous { get; set; }
        public Boolean encashable { get; set; }
        public string effectiveFrom { get; set; }
        public string maxLeavesToForward { get; set; }
        public string alreadyTaken { get; set; }
        public string balance { get; set; }
    }
}
