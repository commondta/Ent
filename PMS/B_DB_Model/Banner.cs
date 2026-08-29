using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class Banner : BaseModel
    {
        [Required]
        public string Image { get; set; } = String.Empty;
        public string? Thumbnail { get; set; }
        public int? BlockId { get; set; }
        public Block? Block { get; set; }
        public int? PropertyTypeId { get; set; }
        public PropertyType? PropertyType { get; set; }
        public string? BannerType { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

    }
}
