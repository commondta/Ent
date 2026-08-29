using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class NDCRequestForDealer : BaseModel
    {
        [ForeignKey("NDCRequestTypeID")]
        public int? NDCRequestTypeID { get; set; }
        public NDCRequestType? NDCRequestType { get; set; }

        [ForeignKey("TransferTypeID")]
        public int? TransferTypeID { get; set; }
        public TransferType? TransferType { get; set; }

        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }

        [ForeignKey("DealerId")]
        public int? DealerId { get; set; }
        public Dealer? Dealer { get; set; }

        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        [Required]
        public bool Outstation { get; set; }

        [Required]
        public DateTime SlotTime { get; set; }

        public DateTime? ValidityDate { get; set; }

        [NotMapped]
        public string? PropertyNo { get; set; }

        public virtual ICollection<NDCRequestForDealerCharges>? NDCRequestForDealerCharges { get; set; }
        public virtual ICollection<NDCRequestForDealerAttachments>? NDCRequestForDealerAttachments { get; set; }
    }

    public class NDCRequestForDealerCharges : BaseModel
    {
        public string ChargeName { get; set; }

        [DataType("decimal(18,2)")]
        public decimal Amount { get; set; }

        [ForeignKey("NDCRequestForDealerId")]
        public int? NDCRequestForDealerId { get; set; }
        public NDCRequestForDealer? NDCRequestForDealer { get; set; }
    }
    public class NDCRequestForDealerAttachments : BaseModel
    {
        public string DoucmentName { get; set; }
        public string Document { get; set; }

        [ForeignKey("NDCRequestForDealerId")]
        public int? NDCRequestForDealerId { get; set; }
        public NDCRequestForDealer? NDCRequestForDealer { get; set; }
    }
}
