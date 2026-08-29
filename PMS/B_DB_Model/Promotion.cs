
using System.ComponentModel.DataAnnotations;


namespace B_DB_Model
{
    public class Promotion : BaseModel
    {
        [Required]
        public string Image { get; set; } = string.Empty;
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        public string? PromotionType { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
