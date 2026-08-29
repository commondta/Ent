using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class NDCRequestForMember : BaseModel
    {
        public bool? IsNDCRequestForMemberRequested { get; set; }
        public bool? IsNDCRequestForMemberApproved { get; set; }
        public bool? IsSurrenderRequested { get; set; }
        public string? DealerCode { get; set; }
        public string? EstateName { get; set; }
        public string? DealerName { get; set; }
        public bool? Processing { get; set; }

        public string? Block { get; set; }
        public string? Category { get; set; }
        public string? Size { get; set; }
        public string? PossessionStatus { get; set; }
        public string? ConstrucationStatus { get; set; }
        public bool? IsRequestedClosed { get; set; }
        [MaxLength(100)]
        public string? TransferPurpose { get; set; }
        public string? NDCRequestType { get; set; }

        [MaxLength(100)]
        public string? ApplyStation { get; set; }

        [ForeignKey("TransferTypeID")]
        public int? TransferTypeID { get; set; }
        public TransferType? TransferType { get; set; }

        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }

        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        [Required]
        public bool Outstation { get; set; }
      
        public DateTime? SlotDate { get; set; }
        
        public string? SlotHour { get; set; }

        
        public string? SlotMintues { get; set; }

        public DateTime? ValidityDate { get; set; }
        public string? Day { get; set; }
        public bool? SignVerification { get; set; }
        
        public bool? IsCanceled { get; set; }
        public bool? IsRead { get; set; }
        [MaxLength(20)]
        public string? BCApprovalRequired { get; set; }
        [MaxLength(20)]
        public string? OldTranser { get; set; }

        public virtual ICollection<NDCRequestForMemberCharges>? NDCRequestForMemberCharges { get; set;}
        public virtual ICollection<NDCRequestForMemberAttachments>? NDCRequestForMemberAttachments { get; set;}
    }

    public class NDCRequestForMemberCharges : BaseModel
    {
        public string ChargeName { get; set; }

        [DataType("decimal(18,2)")]
        public decimal Amount { get; set; }

        [ForeignKey("NDCRequestForMemberId")]
        public string? SapAccount { get; set; }
        public int? NDCRequestForMemberId { get; set; }
        public NDCRequestForMember? NDCRequestForMember { get; set; }
    }
    public class NDCRequestForMemberAttachments : BaseModel
    {
        public string? DoucmentName { get; set; }
        public string? Document { get; set; }
        public string? Remarks { get; set; }

        [ForeignKey("NDCRequestForMemberId")]
        public int? NDCRequestForMemberId { get; set; }
        public NDCRequestForMember? NDCRequestForMember { get; set; }
    }
}
