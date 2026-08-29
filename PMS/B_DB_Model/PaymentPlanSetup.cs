using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class PaymentPlanSetup : BaseModel
    {
        public int? Code { get; set; }

        public string PlanType { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public int? PhaseId { get; set; }
        public int? RealEsateId { get; set; }
        public int? ProjectId { get; set; }
        public int? BlockId { get; set; }
        public int? CategoryId { get; set; }
        public int? TypeId { get; set; }
        public int? NatureId { get; set; }

        public string Status { get; set; } = string.Empty;

        [DataType("decimal(18,2)")]
        public decimal LandCost { get; set; }
        public int NUmberOfInstallment { get; set; }

        public int InstallmentDays { get; set; }
        [DataType("decimal(18,2)")]
        public decimal? SurChargePerDay { get; set; }
        [DataType("decimal(18,2)")]
        public decimal? GrancePeriodFine { get; set; }
        [DataType("decimal(18,2)")]
        public decimal? Total { get; set; }

        //DTO
        [NotMapped]
        public string? RealStateTypeName { get; set; }
        [NotMapped]
        public string? ProjectName { get; set; }
        [NotMapped]
        public string? PhaseName { get; set; }
        [NotMapped]
        public string? BlockName { get; set; }
        [NotMapped]
        public string? CategoryName { get; set; }
        [NotMapped]
        public string? TypeName { get; set; }
        [NotMapped]
        public string? NatureName { get; set; }

        //Navigation

        public virtual ICollection<PlanInformation>? PlanInformation { get; set; }
    }

    public class PlanInformation : BaseModel
    {
        public string PaymentType { get; set; } = string.Empty;
        [DataType("decimal(18,2)")]
        public decimal Percentage { get; set; }
        [DataType("decimal(18,2)")]
        public int ChargeTypeId { get; set; } 
        public decimal Amount { get; set; } 
        public int DueDays { get; set; }

        public string PaymentFor { get; set; } = string.Empty;

        public string Remarks { get; set; } = string.Empty;

        //Navigation

        [ForeignKey("PaymentPlanSetupId")]
        public int? PaymentPlanSetupId { get; set; }
        public PaymentPlanSetup? PaymentPlanSetup { get; set; }
    }
}
