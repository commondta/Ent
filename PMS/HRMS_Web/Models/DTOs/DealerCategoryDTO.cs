using System.ComponentModel.DataAnnotations;

namespace HRMS_Web.Models.DTOs
{
    public class DealerCategoryDTO
    {
        public int Id { get; set; }

        [Required]
        public string CategoryCode { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
