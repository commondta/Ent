using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class Deal : BaseModel
    {
        [ForeignKey("DealerId")]
        public int? DealerId { get; set; }
        public virtual Dealer? Dealer { get; set; }
        public string DealName { get; set; } = String.Empty;
        public string DealNature { get; set; } = String.Empty;
        public string DealType { get; set; } = String.Empty;
        public int? QtyProperty { get; set; }
        public DateTime? DealDate { get; set; }
        public DateTime? DealExpDate { get; set; }
        public string? CommissionType { get; set; }
        public decimal? Commission { get; set; }
        public string? RebateType { get; set; }
        public decimal? Rebate { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal? NetReceivable { get; set; }
        public decimal? TotalReceied { get; set; }
        public decimal? OutstandingBalance { get; set; }
        public int? GracePeriod { get; set; }
        public decimal? SurchargePerDay { get; set; }
        public decimal? OneTimePayment { get; set; }
        public decimal? Installment { get; set; }
        public string? Remarks { get; set; }

        public bool? IsDealRequested { get; set; }
        public bool? IsDealApproved { get; set; }

        public virtual ICollection<DealProperty>? DealProperty { get; set; }
    }

    public class DealProperty : BaseModel
    {
        public int? StockId { get; set; }
        public string? RegistrationNo { get; set; } = string.Empty;
        public string? PropertyNo { get; set; } = string.Empty;
        public string? RealStateType { get; set; } = string.Empty;
        public string? Project { get; set; } = string.Empty;
        public string? Block { get; set; } = string.Empty;
        
        public decimal? Rebate { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? NetReceivable { get; set; }
        public decimal? ReceiedAmount { get; set; }
        public decimal? OutstandingBalance { get; set; }
        public string? Remarks { get; set; } = string.Empty;

        [ForeignKey("DealId")]
        public int? DealId { get; set; }
        public virtual Deal? Deal { get; set; }
        public virtual ICollection<DealPaymentPlan>? DealPaymentPlan { get; set; }
    }

    public class DealPaymentPlan : BaseModel
    {
        [ForeignKey("DealPropertyId")]
        public int? DealPropertyId { get; set; }
        public virtual DealProperty? DealProperty { get; set; } 

        public string? ChargeType { get; set; } = string.Empty;
        public decimal? GrossAmount { get; set; }
        public decimal? Rebate { get; set; }
        public decimal? NetAmount { get; set; }
        public string? PaymentMethod { get; set; } = string.Empty;
        public decimal? NetTotal { get; set; }

    }
}
