using System.ComponentModel.DataAnnotations;

namespace HRMS_Web.Models.DTOs
{
    public class AUAprrovalRequestDTO
    {
        [Required]
        public int RequestId { get; set; }

        [Required]
        public int ApprovalUIId { get; set; }
    }

    public class ActiveApprovalUI
    {
        [Required]
        public int SerialNo { get; set; }

        [Required]
        public bool Checked { get; set; }
    }
}
