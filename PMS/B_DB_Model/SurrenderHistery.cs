using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class SurrenderHistery : BaseModel
    {
        //Navigation

        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public virtual StockCreation? StockCreation { get; set; }

        [ForeignKey("DealerId")]
        public int? DealerId { get; set; }
        public virtual Dealer? Dealer { get; set; }
        public DateTime ResurrenderDate { get; set; }
        public int? ExpiryDays { get; set; }
        public string? DealerName { get; set; }
        public string? EstateName { get; set; }
        public string? Status { get; set; } 
        public string? Remarks { get; set; }
        public bool? IsReSurrenderRequest { get; set; }
        public bool? IsReSurrenderApproved { get; set; }
    }
}
