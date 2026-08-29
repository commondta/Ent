using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class FixedChargeBill : BaseModel
    {
        public string Month { get; set; } = string.Empty;
        public string BillFor { get; set; } = string.Empty;
        public string RegistrationNo { get; set; } = string.Empty;
        public bool? IsFixedChargeBillRequested { get; set; }
        public bool? IsFixedChargeBillApproved { get; set; }

        public decimal WTaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? Arrears { get; set; }

        //Navigation
        [ForeignKey("StockCreationID")]
        public int? StockCreationID { get; set; }
        public virtual StockCreation? StockCreation { get; set; }

        public virtual ICollection<FixedChargeBillDetail>? FixedChargeBillDetail { get; set; }
        public virtual ICollection<FixedChargeBillWHApplied>? FixedChargeBillWHApplied { get; set; }

    }

    public class FixedChargeBillDetail : BaseModel
    {
        public string? BillType { get; set; }
        public int? ChargeTypeId { get; set; }
        public string? Description { get; set; }
        public string? SapAccount { get; set; }
        public decimal Unit { get; set; }
        public decimal Amount { get; set; }
        public int Surcharge { get; set; }
        public string? OtherDuesDescription { get; set; }
        public int OtherDuesAmount { get; set; }
        public decimal? SaleTax { get; set; }
        public decimal SaleTaxAmount { get; set; }
        public decimal WTaxAmountLine { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }

        //Navigation
        [ForeignKey("FixedChargeBillId")]
        public int? FixedChargeBillId { get; set; }
        public virtual FixedChargeBill? FixedChargeBill { get; set; }
    }

    public class FixedChargeBillWHApplied : BaseModel
    {
        public string Month { get; set; } = string.Empty;
        public string RegistrationNo { get; set; } = string.Empty;
        public string? TaxCode { get; set; }
        public int? ChargeTypeId { get; set; }
        public decimal NetAmount { get; set; }
        public decimal WHPercentage { get; set; }
        public decimal Amount { get; set; }

        //Navigation
        [ForeignKey("FixedChargeBillId")]
        public int? FixedChargeBillId { get; set; }
        public virtual FixedChargeBill? FixedChargeBill { get; set; }
    }
}
