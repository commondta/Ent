using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class TransferSetReceiving :BaseModel
    {
        [NotMapped]
        public int Ndc1Id { get; set; }

        public bool? IsSitePlanRequested { get; set; }
        public bool? IsSitePlanApproved { get; set; }
        public bool? IsRequestClosed { get; set; }
        public string? SetReceivingStatus { get; set; }
        public string? Block { get; set; }
        public string? Category { get; set; }
        public string? Size { get; set; }
        public string? PossessionStatus { get; set; }
        public string? ConstrucationStatus { get; set; }

        public DateTime? SlotDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string? SlotHour { get; set; }
        public string? SlotMintues { get; set; }
        public string? NDCRequestType { get; set; }
        public string? TransferType { get; set; }
        public string? Day { get; set; }
        public string? DealerCode { get; set; }
        public string? EstateName { get; set; }
        public string? DealerName { get; set; }
        public string? Depositor { get; set; }
        [MaxLength(100)]
        public string? TransferPurpose { get; set; }

        [MaxLength(100)]
        public string? ApplyStation { get; set; }

        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }

        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        public virtual ICollection<TransferSetReceivingAttachments>? TransferSetReceivingAttachments { get; set; }
    }

    public class TransferSetReceivingAttachments : BaseModel
    {
        public string? DoucmentName { get; set; }
        public string? Document { get; set; }
        public string? Remarks { get; set; }

        [ForeignKey("TransferSetReceivingId")]
        public int? TransferSetReceivingId { get; set; }
        public TransferSetReceiving? TransferSetReceiving { get; set; }
    }
}
