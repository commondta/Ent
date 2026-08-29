using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class TestApproval : BaseModel
    {
        [ForeignKey("ApprovalSetupId")]
        public int ApprovalSetupId { get; set; }
        public ApprovalSetup? ApprovalSetup { get; set; }

        public int RequestId { get; set; }

        public int ApprovalUIId { get; set; }

        public int StageNo { get; set; }

        public int NumberOfApprovalRequired { get; set; }

        public int UserId { get; set; }

        public string? UserDesignation { get; set; }

        public string ApprovalStatus { get; set; }

        public bool? Is_Assigned { get; set; }

        public DateTime? AssignedDateTime { get; set; }

        public DateTime? ActionDateTime { get; set; }

        public string? LastActionComment { get; set; }

        public bool? IsCancelled { get; set; }
    }
}
