using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class BulkDeal : BaseModel
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

        public virtual ICollection<BulkDealProposePlan>? BulkDealProposePlan { get; set; }
        public virtual ICollection<BulkDealProperty>? BulkDealProperty { get; set; }
        public virtual ICollection<BulkPaymentSchedule>? BulkPaymentSchedule { get; set; }
    }

    public class BulkDealProposePlan : BaseModel
    {
        public int? CategoryId { get; set; }
        public int? Quantity { get; set; } 
        public decimal? UnitPrice { get; set; } 
        public decimal? TotalAmount { get; set; } 
        [ForeignKey("BulkDealId")]
        public int? BulkDealId { get; set; }
        public virtual BulkDeal? BulkDeal { get; set; }
    }
    public class BulkDealProperty : BaseModel
    {
        public int? StockId { get; set; }
        public string? RegistrationNo { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;
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

        [ForeignKey("BulkDealId")]
        public int? BulkDealId { get; set; }
        public virtual BulkDeal? BulkDeal { get; set; }

    }
    public class BulkPaymentSchedule : BaseModel
    {
        public DateTime? DueDate { get; set; }
        public decimal? Amount { get; set; }
        public string? Remarks { get; set; }
        
        [ForeignKey("BulkDealId")]
        public int? BulkDealId { get; set; }
        public virtual BulkDeal? BulkDeal { get; set; }

    }

}
