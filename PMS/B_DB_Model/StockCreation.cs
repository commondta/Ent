using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_DB_Model
{
    public class StockCreation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]

        public DateTime Created_at { get; set; }

        //Navigation  waiting abdul wasy availability to apply navigation

        //[ForeignKey("Real_EstateID")]
        //public int? Real_EstateID { get; set; }
        //public Real_Estate? Real_Estate { get; set; }
        public string? RealStateType { get; set; }

        //[ForeignKey("ProjectID")]
        //public int? ProjectID { get; set; }
        //public Project? Project { get; set; }

        public string? Project { get; set; }
        //[ForeignKey("PhaseID")]
        //public int? PhaseID { get; set; }
        //public Phase? Phase { get; set; }
        public string? Phase { get; set; }

        //[ForeignKey("BlockID")]
        //public int? BlockID { get; set; }
        //public Block? Block { get; set; }

        public string? Block { get; set; }

        //[ForeignKey("CategoryID")]
        //public int? CategoryID { get; set; }
        //public Category? Category { get; set; }
        public string? Category { get; set; }

        //[ForeignKey("TypeID")]
        //public int? TypeID { get; set; }
        //public Type? Type { get; set; }
        public string? Type { get; set; }

        //[ForeignKey("NatureID")]
        //public int? NatureID { get; set; }
        //public Nature? Nature { get; set; }

        [ForeignKey("DealerId")]
        public int? DealerId { get; set; }
        public Dealer? Dealer { get; set; }

        [ForeignKey("MemberProfileId")]
        public int? MemberProfileId { get; set; }
        public MemberProfile? MemberProfile { get; set; }

        public string? InventoryStatus { get; set; }
        public string? Nature { get; set; }
        public string? Finishing { get; set; }
        [MaxLength(20)]
        public string? Feature { get; set; }
        public string? Floor { get; set; }
        public string? ActualSize { get; set; }
        public string? ActualSizeUnit { get; set; }
        public string? Status { get; set; }
        public string? User { get; set; }
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
        public DateTime? DemarcationExpireOn { get; set; }
        public DateTime? DemarcationFileSubmitedDate { get; set; }
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
        public string? LDAPlotNo { get; set; }
        public string? LDAAreaSize { get; set; }
        public string? Almt { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public decimal? DiscountPercent { get; set; }

        public string? PropertyStatus { get; set; }
        public string? CombinedImageUrl { get; set; }

        [MaxLength(100)]
        public string? CaseCode { get; set; }
        [MaxLength(100)]
        public string? AffidavitCode { get; set; }
        [MaxLength(100)]
        public string? SaleDeedNo { get; set; }
        public DateTime? SaleDeedDate { get; set; }
        [MaxLength(100)]
        public string? Mouza { get; set; }
        [MaxLength(100)]
        public string? AllocationNo { get; set; }

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

        public int? Created_By { get; set; }
        public DateTime Updated_at { get; set; }
        public int Updated_By { get; set; }
        public bool is_active { get; set; } = true;
        public bool is_deleted { get; set; } = false;

        public int MembershipFee { get; set; } = 0;
        public int MiscCharges { get; set; } = 0;
        public int MaintenceAdvanceBillPaid { get; set; } = 0;

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

        public string? PossessionNo { get; set; }
        public string? PossessionSurveyorName { get; set; }
        public string? OwnerName { get; set; }

        public string? SurveyorName { get; set; }
        public string? BuildingControlDirectorName { get; set; }

        [MaxLength(100)]
        public string? BillPrintRegistrationNo { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? BillPrintPropertyNo { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? BillPrintName { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? BillPrintAddress { get; set; } = string.Empty;

        //DTo items

        [NotMapped] public bool? IsConfirmed { get; set; }

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

        [NotMapped]
        public DateTime? ClearanceDate { get; set; }

        [NotMapped]
        public int? RequestId { get; set; }
        [NotMapped]
        public int? ApprovalUIID { get; set; }
        [NotMapped]
        public decimal? BulkDealAmount { get; set; }

        [NotMapped]
        public string? LastModifiedUser { get; set; }

        [NotMapped]
        public List<MemberName>? MemberNames { get; set; } = new List<MemberName>();

        public virtual ICollection<TanantDetail>? TanantDetail { get; set; }
        public virtual ICollection<MapApprovalHistery>? MapApprovalHistory { get; set; }
    }

    public class MemberName
    {
        public string? MemeberName { get; set; }
        public string? Relationhipwith { get; set; }
        public string? RelationName { get; set; }
        public string? Cnic { get; set; }
    }

    public class MapApprovalHistery : BaseModel
    {
        public string? Description { get; set; }
        public string? Architecture { get; set; }
        public string? MapType { get; set; }
        public DateTime? DateofSubmission { get; set; }
        public string? ClientStatus { get; set; }
        public int? Stage { get; set; }
        public int? ClientStage { get; set; }
        public DateTime? DateofFeedback { get; set; }
        public string? Attachments { get; set; }
        public string? ArchRemarks { get; set; }
        public string? ClientRemarks { get; set; }
        public bool? Is_Checked { get; set; }

        [NotMapped]
        public string? FindMode { get; set; }

        [NotMapped]
        public string? RedesignMappApproved { get; set; }

        [NotMapped]
        public bool? Is_MappApproved { get; set; }

        [NotMapped]
        public string? CoveredArea { get; set; }

        [ForeignKey("StockCreationID")]
        public int? StockCreationID { get; set; }
        public StockCreation? StockCreation { get; set; }
    }
        public class TanantDetail : BaseModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        [MaxLength(100)]
        public string? Name { get; set; } 
        [MaxLength(20)]
        public string? Mobile { get; set; } 
        [MaxLength(20)]
        public string? Cnic { get; set; }
        public string? Attachment { get; set; }
        [MaxLength(5000)]
        public string? Remarks { get; set; }

        //Navigation

        [ForeignKey("StockCreationID")]
        public int? StockCreationID { get; set; }
        public StockCreation? StockCreation { get; set; }

    }
}
