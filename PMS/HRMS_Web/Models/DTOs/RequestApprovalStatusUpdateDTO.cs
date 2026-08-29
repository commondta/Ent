using System.ComponentModel.DataAnnotations;

namespace HRMS_Web.Models.DTOs
{
    public class RequestApprovalStatusUpdateDTO
    {
        [Required]
        public int RequestId { get; set; }
        [Required]
        public int ApprovalUIId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int IsApproved{ get; set; }
        public string? Comment { get; set; }
    }
}
