using B_DB_Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS_Web.Models.DTOs
{
    public class PreSaleInboxDTO
    {
        public int Id { get; set; } 
        public string? MemberName { get; set; } = string.Empty;
        public string? Cnic { get; set; } = string.Empty;
        public string? MobileNo { get; set; } = string.Empty;
        public string? ReferedBy { get; set; } = string.Empty;
        public string? DealerName { get; set; } = string.Empty;
        public string? RegistrationNo { get; set; }= string.Empty;
        public string? PropertyNo { get; set; } = string.Empty;
        public int? ReciptId { get; set; } = 0;
        public int? RequestId { get; set; }
        public int? ApprovalUIID { get; set; }
    }
}
