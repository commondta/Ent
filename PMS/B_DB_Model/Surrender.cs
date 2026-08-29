using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class Surrender : BaseModel
    {
        //Navigation

        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public virtual StockCreation? StockCreation { get; set; }

        [ForeignKey("DealerId")]
        public int? DealerId { get; set; }
        public virtual Dealer? Dealer { get; set; }

        public string? ImageURL { get; set; } 

        public DateTime ResurrenderDate { get; set; }

        public int? ExpiryDays { get; set; }

        public string? DealerName { get; set; }
        public string? EstateName { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; }
        public bool? IsSurrenderRequest { get; set; }
        public bool? IsSurrenderApproved { get; set; }
        public bool? IsRequestClosed { get; set; }
        [NotMapped]
        public virtual ICollection<ResurrenderCharges>? ResurrenderCharges { get; set; }

    }

    public class ResurrenderCharges : BaseModel
    {
        public string ChargeName { get; set; }
        [DataType("decimal(18,2)")]
        public decimal Amount { get; set; }
        public string? SapAccount { get; set; }
        [ForeignKey("SurrenderId")]
        public int? SurrenderId { get; set; }
        public Surrender? Surrender { get; set; }
    }
}
