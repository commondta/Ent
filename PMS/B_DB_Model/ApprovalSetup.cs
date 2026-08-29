using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class ApprovalSetup : BaseModel
    {
        [ForeignKey("ApprovalUIId")]
        public int ApprovalUIId { get; set; }
        public ApprovalUI? ApprovalUI { get; set; }

        public int StageNo { get; set; }

        public int NumberOfApprovalRequired { get; set; }

        public virtual ICollection<ApprovalUsers>? ApprovalUsers { get; set; }  
    }

    public class ApprovalUsers : BaseModel
    {
        [ForeignKey("ApprovalSetupId")]
        public int ApprovalSetupId { get; set; }
        public ApprovalSetup? ApprovalSetup { get; set; }

        //UserId will be here Foreign Key after user module added
        public int UserId { get; set; }

        public string? UserDesignation { get; set; }
    }
}
