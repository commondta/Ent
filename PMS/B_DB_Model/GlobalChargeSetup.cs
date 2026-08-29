using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class GlobalChargeSetup : BaseModel
    {
        public string? ChargeStatus { get; set; }
        public string? ConstructionStatus { get; set; }
        public string? GeneratorUnitType { get; set; }
        public bool? PossessionStatus { get; set; }
        public bool? GracePeriod { get; set; }
        public string? TaxStatus { get; set; }
        public string? NDCRequestType { get; set; }
        public string? NDCTransferType { get; set; }
        public bool? NDCProcessing { get; set; }
        public bool? RegistryVerification { get; set; }
        public bool? FBR236C { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Description { get; set; }
        public string? FileRequestType { get; set; }
        public string? Redesign { get; set; } = string.Empty;

        // Navigation
        [ForeignKey("GlobalChargeGroupId")]
        public int? GlobalChargeGroupId { get; set; }
        public GlobalChargeGroup? GlobalChargeGroup { get; set; }

         public int? RealStateTypeId { get; set; }
         public int? ProjectId { get; set; }
         public int? PhaseId { get; set; }
         public int? BlockId { get; set; }
         public int? CategoryId { get; set; }
         public int? PropertyTypeId { get; set; }
         public int? NatureId { get; set; }
        public string? Sector { get; set; }
        public virtual ICollection<GlobalChargeDetail> GlobalChargeDetail { get; set; }
    }

    public class GlobalChargeDetail : BaseModel
    {
        public int? Code { get; set; }
        public string? ChargeType { get; set; }
        public string? SapAccount { get; set; }

        public string? Description { get; set; }

        [DataType("decimal(18,2)")]
        public decimal? Rate { get; set; }
        public decimal? Percentage { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public bool? Status { get; set; }
        public bool? WHStatus { get; set; }
        public bool? MultiplyBySize { get; set; }
        public bool? Yearly { get; set; }

        //Navigation

        [ForeignKey("GlobalChargeSetupId")]
        public int? GlobalChargeSetupId { get; set; }
        public GlobalChargeSetup? GlobalChargeSetup { get; set; }
    }
}
