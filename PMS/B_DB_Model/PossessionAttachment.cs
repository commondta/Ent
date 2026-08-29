using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class PossessionAttachment
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(2000)]
        public string? Remarks { get; set; } = string.Empty;
        public string? Piture { get; set; } = string.Empty;
        public int? StockCreationId { get; set; }
    }
}
