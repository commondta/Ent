using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class AmalgamationDetails
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public virtual StockCreation? StockCreation { get; set; }

        [ForeignKey("AmalgamationId")]
        public int AmalgamationId { get; set; }
        public virtual Amalgamation? Amalgamation { get; set; }
    }
}
