using System.ComponentModel.DataAnnotations;

namespace HRMS_Web.Models.DTOs
{
    public class StockCeationAprrovalDTO
    {
        public int ApprovalUIId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int IsApproved { get; set; }
        public string? Comment { get; set; }

        public List<stockCreationResquestIds>? stockCreationResquestIds { get; set; }


    }

    public class stockCreationResquestIds
    {
        public int RequestId { get; set; }
    }
}
