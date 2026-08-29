using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class PreSale : BaseModel
    {
        public string? Status { get; set; } 
        public string? MemberName { get; set; } 
        public string? Cnic { get; set; } 
        public string? Address { get; set; }
        public string? Email { get; set; } 
        public string? MobileNo { get; set; } 
        public string? ByCareOf { get; set; } 
        public string? ReferedBy { get; set; }
        public string? DealerCode { get; set; } 
        public string? DealerName { get; set; } 
        public string? SaleBy { get; set; } 
        public string? TranscationType { get; set; } 
        public string? Remarks { get; set; } 
        public string? PlanCode { get; set; } 
        
        public int? OneTimePayment { get; set; }
        public int? Installments { get; set; }
        public int? TotalCost { get; set; }
        public int? TotalRebate { get; set; }
        public int? NetCost { get; set; }

        //Navigation

        [ForeignKey("StockCreationId")]
        public int? StockCreationId { get; set; }
        public virtual StockCreation? StockCreation { get; set; }

        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public virtual MemberProfile? MemberProfile { get; set; }

        [ForeignKey("DealerId")]
        public int? DealerId { get; set; }
        public virtual Dealer? Dealer { get; set; }

        public virtual ICollection<TermsConditions>? TermsConditions { get; set; }
        public virtual ICollection<PaymentPlan>? PaymentPlan { get; set; }

    }

    public class TermsConditions
    {
        [Key]
        public int Id { get; set; }
        public string? TranscationType { get; set; } 

        [ForeignKey("PreSaleId")]
        public int? PreSaleId { get; set; }
        public PreSale? PreSale { get; set; }
    }

    public class PaymentPlan : BaseModel
    {
        public string? PaymentType { get; set; } 
        [DataType("decimal(18,2)")]
        public decimal Amount { get; set; }
        [DataType("decimal(18,2)")]
        public decimal? Rebate { get; set; }
        [DataType("decimal(18,2)")]
        public decimal? NetAmount { get; set; }
        public int? ChargeTypeId { get; set; }
        public int? Days { get; set; }
        public string? PaymentFor { get; set; } 
        public string? PaymentMethod { get; set; } 
        [ForeignKey("PreSaleId")]
        public int? PreSaleId { get; set; }
        public PreSale? PreSale { get; set; }
    }
}
