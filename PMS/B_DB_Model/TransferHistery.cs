using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class TransferHistery : BaseModel
    {
        public string? TransferFromImage { get; set; } 
        public string? TransferToImage { get; set; } 
        public string? CombineImage { get; set; } 
        public DateTime? LetterDate { get; set; } = DateTime.Now;
        public string? Remarks { get; set; } 
        public bool? IsHold { get; set; }
        public bool? IsTransferRequested { get; set; }
        public bool? IsTransferApproved { get; set; }
        public bool? IsRequestClosed { get; set; }
        public bool? IsGovtProcessingTaxRequested { get; set; }
        public bool? IsGovtProcessingTaxApproved { get; set; }
        public int? ReciptPrpcessingId { get; set; }
        [MaxLength(100)]
        public string? ApplyStation { get; set; }
        [MaxLength(100)]
        public string? TransferPurpose { get; set; }
        public string? SellerName { get; set; }
        public string? SellerCnic { get; set; }
        public string? DealerCode { get; set; }
        public string? EstateName { get; set; }
        public string? DealerName { get; set; }
        [MaxLength(100)]
        public string? SellerRepresentativeName { get; set; }
        [MaxLength(100)]
        public string? SellerRepresentativeRelationshipWith { get; set; }
        [MaxLength(20)]
        public string? SellerRepresentativeCnic { get; set; }
        [MaxLength(100)]
        public string? BuyerRepresentativeName { get; set; }
        [MaxLength(100)]
        public string? BuyerRepresentativeRelationshipWith { get; set; }
        [MaxLength(20)]
        public string? BuyerRepresentativeCnic { get; set; }
        [MaxLength(100)]
        public string? NDCRequestType { get; set; }
        [MaxLength(100)]
        public string? TransferType { get; set; }
        [MaxLength(100)]
        public string? SellerStation { get; set; }
        [MaxLength(100)]
        public string? BuyerStation { get; set; }

        [MaxLength(100)]
        public string? InternalDocumentNo { get; set; }

        [MaxLength(100)]
        public string? InternalDocumentNoOptional { get; set; }

        public string? LegalHeireType { get; set; }
        public string? LagalHeireContent { get; set; }

        [NotMapped]
        public int? BuyerId { get; set; }
        [NotMapped]
        public int? SellerId { get; set; }
        [NotMapped]
        public string? SellerRelationshipWith { get; set; }
        [NotMapped]  
        public string? SellerPermanentAddress { get; set; }
        [NotMapped]
        public string? SellerCurrentAddress { get; set; }

        [NotMapped]
        public string? SellerMobileNo { get; set; }

        [NotMapped]
        public string? BuyerName { get; set; }
        [NotMapped]
        public string? BuyerMobileNo { get; set; }
        [NotMapped]  
        public string? BuyerCnic { get; set; }
        [NotMapped]  
        public string? BuyerRelationshipWith { get; set; }
        [NotMapped]  
        public string? BuyerPermanentAddress { get; set; }
        [NotMapped]
        public string? BuyerCurrentAddress { get; set; }
        [NotMapped]
        public string? SellerMembershipNo { get; set; }
        [NotMapped]
        public string? BuyerMembershipNo { get; set; }

        public string? Statement { get; set; }



        //Navigations
        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }

        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }

        public virtual ICollection<TransferHisteryJointMember>? TransferHistoryJointMember { get; set; }
        public virtual ICollection<TransferHisteryNominee>? TransferHistoryNominee { get; set; }
        public virtual ICollection<TransferHisteryAttachments>? TransferHistoryAttachments { get; set; }

        [NotMapped]
        public List<TransferSetReceivingAttachments>? TransferSetReceivingAttachments { get; set; }

        [NotMapped]
        public virtual ICollection<TransferAttachments>? TransferAttachments { get; set; }
        [NotMapped]
        public virtual ICollection<TransferReceiptJointMember>? TransferReceiptJointMember { get; set; }
        [NotMapped]
        public virtual ICollection<TransferReceiptNominee>? TransferReceiptNominee { get; set; }
        [NotMapped]
        public virtual ICollection<SellerTaxes>? SellerTaxes { get; set; }
        [NotMapped]
        public virtual ICollection<BuyerTaxes>? BuyerTaxes { get; set; }
    }

    public class TransferHisteryJointMember : BaseModel
    {

        public string Name { get; set; } = String.Empty;
        public string Relationship { get; set; } = String.Empty;
        public string CNIC { get; set; } = String.Empty;
        public string Mobile { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        [NotMapped]
        public string ImageURL { get; set; } = String.Empty;

        [ForeignKey("TransferHisteryId")]
        public int? TransferHisteryId { get; set; }
        public TransferHistery? TransferHistery { get; set; }
    }

    public class TransferHisteryNominee : BaseModel
    {

        public string Name { get; set; } = String.Empty;
        public string Relationship { get; set; } = String.Empty;
        public string CNIC { get; set; } = String.Empty;
        public string Mobile { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;

        [ForeignKey("TransferHisteryId")]
        public int? TransferHisteryId { get; set; }
        public TransferHistery? TransferHistery { get; set; }
    }

    public class TransferHisteryAttachments : BaseModel
    {
        public string DoucmentName { get; set; }
        public string Document { get; set; }
        public string Remarks { get; set; }

        [ForeignKey("TransferHisteryId")]
        public int? TransferHisteryId { get; set; }
        public TransferHistery? TransferHistery { get; set; }
    }
}
