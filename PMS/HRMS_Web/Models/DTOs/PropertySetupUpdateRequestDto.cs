using System.ComponentModel.DataAnnotations;

namespace HRMS_Web.Models.DTOs
{
    public class PropertySetupUpdateRequestDto
    {
        public int StockCreationId { get; set; }
        public string? PropertyNo { get; set; }
        public string? PhaseId { get; set; }
        public string? RealEsateId { get; set; }
        public string? ProjectId { get; set; }
        public string? BlockId { get; set; }
        public string? NatureId { get; set; }
        public string? CategoryId { get; set; }
        public string? TypeId { get; set; }
        public string? Feature { get; set; }
        public string? PrefixProperty { get; set; }
        public string? PropertyStatus { get; set; }
        public decimal? DiscountPercent { get; set; } = 0;
        public string? ActualSize { get; set; }
        public decimal? coveredArea { get; set; }
        public bool? PossessionStatus { get; set; }
        public string? ConstructionStatus { get; set; }
        public DateTime? GracePeriodDate { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? CaseCode { get; set; }
        public string? AffidavitCode { get; set; }
        public string? SaleDeedNo { get; set; }
        public DateTime? SaleDeedDate { get; set; }
        public string? Mouza { get; set; }
        public string? AllocationNo { get; set; }
        public int MembershipFee { get; set; } = 0;
        public int MiscCharges { get; set; } = 0;

        // =====================================================
        // PART I - Transfer & Record Branch
        // =====================================================

        public string? TransferRecordOfficerName { get; set; }
        public string? TransferRecordDirectorName { get; set; }


        // =====================================================
        // Plot Demarcation / Boundary Details
        // =====================================================

        public string? FrontSide { get; set; }
        public string? RearSide { get; set; }
        public string? LeftSide { get; set; }
        public string? RightSide { get; set; }

        public string? FrontBoundary { get; set; }
        public string? RearBoundary { get; set; }
        public string? LeftBoundary { get; set; }
        public string? RightBoundary { get; set; }

        // =====================================================
        // Plot Area Details
        // =====================================================

        public decimal? StandardAreaOfPlot { get; set; }
        public decimal? AreaOfPlot { get; set; }
        public decimal? ExcessArea { get; set; }
        public decimal? LessArea { get; set; }

        public string? ApprovedMinSheetReferenceNo { get; set; }

        // =====================================================
        // Plot Features
        // =====================================================

        public bool? IsCornerPlot { get; set; }
        public bool? IsParkFacing { get; set; }
        public bool? IsMainBoulevard { get; set; }


        // =====================================================
        // PART III - Finance Branch
        // =====================================================

        public DateTime? DuesClearedTillDate { get; set; }
        public string? NdcNo { get; set; }
        public string? NdcType { get; set; }

        public string? FinanceOfficerName { get; set; }
        public string? FinanceDirectorName { get; set; }

        // =====================================================
        // PART IV - Possession / Handover
        // =====================================================

        public DateTime? PossessionHandedOverOn { get; set; }
        public string? PossessionSurveyorName { get; set; }
        public string? PossessionNo { get; set; }
        public string? OwnerName { get; set; }

        public string? SurveyorName { get; set; }
        public string? BuildingControlDirectorName { get; set; }

        public int? ModifiedBy { get; set; }
        public string? LastModifiedUserName { get; set; }
    }

}
