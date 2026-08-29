using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class MeterBillGeneration : BaseModel
    {
        public string Month { get; set; } = string.Empty;
        public string BillFor { get; set; } = string.Empty;
        public string ChargesStatus { get; set; } = string.Empty;
        public bool? IsChangedFromIndivualBill { get; set; }
        public bool? IsMeterBillGenerationRequested { get; set; }
        public bool? IsMeterBillGenerationApproved { get; set; }

        public virtual ICollection<MeterBillGenerationDetail>? MeterBillGenerationDetail { get; set; }

    }

    public class MeterBillGenerationDetail : BaseModel
    {
        public string Month { get; set; } = string.Empty;
        public int? ChargeTypeId { get; set; }
        public string? ChargeType { get; set; }
        public string? SapAccount { get; set; }
        public string PropertyNo { get; set; } = string.Empty;
        public string RegistrationNo { get; set; } = string.Empty;
        public string MeterNo { get; set; } = string.Empty;
        public string? PreviousReading { get; set; }
        public string? CurrentReading { get; set; }
        public decimal TotalUnitConsumed { get; set; }
        public decimal PerUnitRate { get; set; }
        public decimal FuelAdjustedUnits { get; set; }
        public decimal FuelAdjustment { get; set; }
        public decimal Amount { get; set; }
        public int Surcharge { get; set; }
        public string OtherDuesDescription { get; set; } = string.Empty;
        public int OtherDuesAmount { get; set; }
        public decimal SaleTax { get; set; }
        public decimal SaleTaxAmount { get; set; }
        public decimal WTaxAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal? Arrears { get; set; }
        public decimal? OutstandingBalance { get; set; }

        //Navigation
        [ForeignKey("MeterBillGenerationId")]
        public int? MeterBillGenerationId { get; set; }
        public virtual MeterBillGeneration? MeterBillGeneration { get; set; }
    }
}
