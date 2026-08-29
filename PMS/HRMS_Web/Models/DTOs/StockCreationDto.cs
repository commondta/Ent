using B_DB_Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace HRMS_Web.Models.DTOs
{
    public class StockCreationDto
    {
        public DateTime Created_at { get; set; } = DateTime.Now;
        public string RealStateType { get; set; }
        public string Project { get; set; }
        public string Phase { get; set; }
        public string Block { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }

        public int? DealerId { get; set; }
        public int? MemberProfileId { get; set; }

        public string Nature { get; set; }
        public string Finishing { get; set; }
        public string Floor { get; set; }
        public string ActualSize { get; set; }
        public string ActualSizeUnit { get; set; }
        public string Status { get; set; }
        public string User { get; set; }

        public string? RegistrationNo { get; set; }
        public string? PropertyNo { get; set; }

        public string? PrefixRegistration { get; set; }
        public int? numForRegistration { get; set; }
        public string? postfixForRegistration { get; set; }

        public int? Quantity { get; set; }
        public decimal? coveredArea { get; set; }

        public DateTime? ClearanceOn { get; set; }
        public string? MemberTaxStatus { get; set; }

        public DateTime? PossessionEffectDate { get; set; }
        public bool? PossessionStatus { get; set; }

        public bool? UnderLitigation { get; set; }
        public string? ConstracutionStatus { get; set; }
        public string? GeneratorUnitType { get; set; }

        public bool? IsBillGenerationEnabled { get; set; }
        public bool? IsSaleTaxEnabled { get; set; }
        public bool? IsWithHoldingTaxEnabled { get; set; }

        public DateTime? GrancePeriodForBillGenration { get; set; }

        public string? Location { get; set; }
        public string? Street { get; set; }

        public string? PrefixProperty { get; set; }
        public int? numForProperty { get; set; }
        public string? postfixForProperty { get; set; }

        public bool? Is_StockCreationRequested { get; set; }
        public bool? Is_StockCreationApproved { get; set; }

        public bool? Is_DemarcationRequested { get; set; }
        public bool? Is_ClearnceRequested { get; set; }
        public bool? Is_MapApprovalRequested { get; set; }

        public bool? Is_DemarcationApproved { get; set; }
        public bool? Is_ClearnceApproved { get; set; }
        public bool? Is_MapApprovalApproved { get; set; }

        public bool? Is_ConstructionSecurityRequested { get; set; }
        public bool? Is_ConstructionSecurityApproved { get; set; }

        public bool? Is_ConstructionMonitoringRequested { get; set; }
        public bool? Is_ConstructionMonitoringApproved { get; set; }

        public bool? Is_PossessionRequested { get; set; }
        public bool? Is_PossessionApproved { get; set; }

        public bool? Is_DemarcationFormRequested { get; set; }
        public bool? Is_DemarcationFormApproved { get; set; }

        public bool? IsPreSaleRequested { get; set; }
        public bool? IsPreSaleApproved { get; set; }

        public bool? IsBookingRequested { get; set; }
        public bool? IsBookingApproved { get; set; }

        public int Created_By { get; set; }

        public DateTime Updated_at { get; set; }
        public int Updated_By { get; set; }

        public bool is_active { get; set; } = true;
        public bool is_deleted { get; set; } = false;
    }
}
