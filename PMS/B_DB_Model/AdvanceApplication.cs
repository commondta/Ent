using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class AdvanceApplication : BaseModel
    {
        //Navigation
        [ForeignKey("DealId")]
        public int? DealId { get; set; }
        public Deal? Deal { get; set; }

        [ForeignKey("DealerId")]
        public int? DealerId { get; set; }
        public Dealer? Dealer { get; set; }

        public decimal? TotalAmount { get; set; }

        public virtual ICollection<DealAdvanceApplicationHistery>? DealAdvanceApplicationHistory { get; set; }
        public virtual ICollection<DealAdvanceApplicationRecipt>? DealAdvanceApplicationRecipt { get; set; }
    }

    public class DealAdvanceApplicationHistery : BaseModel
    {
        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }
        public decimal? OutstandingBalance { get; set; }
        public decimal? AmountApplied{ get; set; }

        [ForeignKey("AdvanceApplicationId")]
        public int? AdvanceApplicationId { get; set; }
        public AdvanceApplication? AdvanceApplication { get; set; }
    }

    public class DealAdvanceApplicationRecipt : BaseModel
    {
        public string? ReciptNo { get; set; }
        public decimal? Amount { get; set; }

        [ForeignKey("AdvanceApplicationId")]
        public int? AdvanceApplicationId { get; set; }
        public AdvanceApplication? AdvanceApplication { get; set; }
    }
}
