using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class Amalgamation : BaseModel
    {
        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public virtual StockCreation? StockCreation { get; set; }

        [MaxLength(1000)]
        public string? Remarks { get; set; }

        public virtual ICollection<AmalgamationDetails>? AmalgamationDetails { get; set; }
    }
}
