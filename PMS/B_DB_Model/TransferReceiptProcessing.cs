using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class TransferReceiptProcessing : BaseModel
    {
        public string? CombineImage { get; set; }
        public string? BlockName { get; set; }
        public string? RegistrationNo { get; set; }
        public string? CategoryName { get; set; }
        public string? PlotSize { get; set; }
        public string? ConstructionStatus { get; set; }
        public string? Filer { get; set; }
        public string? BuyerName { get; set; }
        public string? ContactNo { get; set; }
        public string? Address { get; set; }
        public string? CNIC { get; set; }
        public string? TransferDate { get; set; }
        public int SellerId { get; set; }
        public int BuyerId { get; set; }
        public int StockCreationId { get; set; }
        public StockCreation? StockCreation { get; set; }
        public string? CoveredArea { get; set; }
        public string? SellerName { get; set; }
        public string? SellerFilerStatus { get; set; }
        public bool? FBRTAX236C { get; set; }
        public bool? RegistryVerification { get; set; }
        public bool? IsGovtProcessingTaxRequested { get; set; }
        public bool? IsGovtProcessingTaxApproved { get; set; }
        public string? PropertyTaxYears { get; set; }
        public string? ConstructedYears { get; set; }
        public DateTime? SlotDate { get; set; }
        public string? SlotHour { get; set; }
        public string? SlotMintues { get; set; }
        public string? NDCRequestType { get; set; }
        public string? TransferType { get; set; }

        public string? LegalHeireType { get; set; }
        public string? LagalHeireContent { get; set; }
        public string? Day { get; set; }
        [MaxLength(100)]
        public string? TransferPurpose { get; set; }
        public string? DealerCode { get; set; }
        public string? EstateName { get; set; }
        public string? DealerName { get; set; }
        [MaxLength(100)]
        public string? ApplyStation { get; set; }
        [MaxLength(100)]
        public string? PaymentMode { get; set; }

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

        [MaxLength(200)]
        public string? SellerStation { get; set; }
        [MaxLength(200)]
        public string? BuyerStation { get; set; }

        [MaxLength(2000)]
        public string? ChangeOverStatement { get; set; }


        [MaxLength(200)]
        public string? LegalHeirType { get; set; }
        [MaxLength(10000)]
        public string? LegalHeirContent { get; set; }

        public string? ChallanNoSellerTaxes { get; set; }
        public string? ChallanNoBuyerTaxes { get; set; }
        [NotMapped]
        public string? ChallanNo { get; set; }

        public virtual ICollection<SellerTaxes>? SellerTaxes { get; set; }
        public virtual ICollection<BuyerTaxes>? BuyerTaxes { get; set; }
        public virtual ICollection<GovtSellerCharges>? GovtSellerCharges { get; set; }
        public virtual ICollection<GovtBuyerCharges>? GovtBuyerCharges { get; set; }

        [NotMapped]
        public virtual ICollection<JointMemberHistoricalData>? JointMemberHistoricalDatas { get; set; }
        public virtual ICollection<TransferReceiptJointMember>? TransferReceiptJointMember { get; set; }
        public virtual ICollection<TransferReceiptNominee>? TransferReceiptNominee { get; set; }
        public virtual ICollection<TransferAttachments>? TransferAttachments { get; set; }

        [NotMapped]
        public List<TransferSetReceivingAttachments>? TransferSetReceivingAttachments { get; set; }
    }

    public class SellerTaxes
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }

        [NotMapped]
        public string? ChargeName { get; set; }

        [DataType("decimal(18,2)")]
        public decimal Amount { get; set; }
        public int? TaxTypeId { get; set; }
        public TaxType? TaxType { get; set; }
        public string? ChallanNo { get; set; }

        [ForeignKey("TransferReceiptProcessingId")]
        public int? TransferReceiptProcessingId { get; set; }
        public TransferReceiptProcessing? TransferReceiptProcessing { get; set; }
    }

    public class BuyerTaxes
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [NotMapped]
        public string? ChargeName { get; set; }

        [DataType("decimal(18,2)")]
        public decimal Amount { get; set; }
        public int? TaxTypeId { get; set; }
        public TaxType? TaxType { get; set; }
        public string? ChallanNo { get; set; }

        [ForeignKey("TransferReceiptProcessingId")]
        public int? TransferReceiptProcessingId { get; set; }
        public TransferReceiptProcessing? TransferReceiptProcessing { get; set; }
    }

    public class GovtSellerCharges 
    {
        [Key]
        public int Id { get; set; }
        public string ChargeName { get; set; }

        [DataType("decimal(18,2)")]
        public decimal Amount { get; set; }
        public string? SapAccount { get; set; }
        public string? InvoiceType { get; set; }

        [ForeignKey("TransferReceiptProcessingId")]
        public int? TransferReceiptProcessingId { get; set; }
        public TransferReceiptProcessing? TransferReceiptProcessing { get; set; }
    }
    public class GovtBuyerCharges
    {
        [Key]
        public int Id { get; set; }
        public string ChargeName { get; set; }

        [DataType("decimal(18,2)")]
        public decimal Amount { get; set; }
        public string? SapAccount { get; set; }
        public string? InvoiceType { get; set; }
        
        [ForeignKey("TransferReceiptProcessingId")]
        public int? TransferReceiptProcessingId { get; set; }
        public TransferReceiptProcessing? TransferReceiptProcessing { get; set; }
    }
    public class TransferAttachments : BaseModel
    {
        public string DoucmentName { get; set; }
        public string Document { get; set; }
        public string Remarks { get; set; }

        [ForeignKey("TransferReceiptProcessingId")]
        public int? TransferReceiptProcessingId { get; set; }
        public TransferReceiptProcessing? TransferReceiptProcessing { get; set; }
    }
    public class TransferReceiptJointMember : BaseModel
    {

        public string Name { get; set; } = String.Empty;
        public string Relationship { get; set; } = String.Empty;
        public string CNIC { get; set; } = String.Empty;
        public string Mobile { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;

        [NotMapped]
        public string ImageURL { get; set; } = String.Empty;

        [ForeignKey("TransferReceiptProcessingId")]
        public int? TransferReceiptProcessingId { get; set; }
        public TransferReceiptProcessing? TransferReceiptProcessing { get; set; }
    }

    public class TransferReceiptNominee : BaseModel
    {

        public string Name { get; set; } = String.Empty;
        public string Relationship { get; set; } = String.Empty;
        public string CNIC { get; set; } = String.Empty;
        public string Mobile { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;

        [ForeignKey("TransferReceiptProcessingId")]
        public int? TransferReceiptProcessingId { get; set; }
        public TransferReceiptProcessing? TransferReceiptProcessing { get; set; }
    }
}

