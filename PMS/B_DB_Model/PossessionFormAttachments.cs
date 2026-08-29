using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class PossessionFormAttachments
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(2000)]
        public string Remarks { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string Piture { get; set; } = string.Empty;

        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }
    }
}
