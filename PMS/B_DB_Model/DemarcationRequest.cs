using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class NewDemarcationRequest : BaseModel
    {
        public string? Status { get; set; }

        public string? ChallanNo { get; set; }

        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        public bool? IsCancelled { get; set; }

        [NotMapped]
        public string? RedesignRequest { get; set; }

        [NotMapped]
        public DateTime? DueDate { get; set; }

        public virtual ICollection<NewDemarcationRequestDetail>? NewDemarcationRequestDetail { get; set; }
    }

    public class NewDemarcationRequestDetail : BaseModel
    {
        public int? ChargeTypeId { get; set; }
        public string? ChargeName { get; set; }
        public int? Rate { get; set; }
        [NotMapped]
        public int? ChargeID { get; set; }
        [NotMapped]
        public int? ChargeSetUpId { get; set; }

        [ForeignKey("NewDemarcationRequestId")]
        public int? NewDemarcationRequestId { get; set; }
        public NewDemarcationRequest? NewDemarcationRequest { get; set; }

    }
}
