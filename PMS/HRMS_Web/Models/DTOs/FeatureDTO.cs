using System.ComponentModel.DataAnnotations;

namespace HRMS_Web.Models.DTOs
{
    public class FeatureDTO
    {
        public int ID { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime Created_at { get; set; }
        public int Created_By { get; set; }
        public DateTime Updated_at { get; set; }
        public int Updated_By { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
    }
}
