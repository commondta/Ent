using Microsoft.EntityFrameworkCore;
using B_DB_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS_Web.Models.DTOs.SPDtos;
using B_DB_Model.SPDtos;

namespace B_DB_Context
{
    public class DataBase_Context : DbContext
    {

        public DataBase_Context(DbContextOptions<DataBase_Context> options) : base(options)
        {
        }

        // Master Forms
        public DbSet<VerificationType> VerificationTypes { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Force> Forces { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Quota> Quota { get; set; }
        public DbSet<Almt> Almt { get; set; }
        public DbSet<MemberCategory> MemberCategorys { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SurchargeSetup> SurchargeSetups { get; set; }
        public DbSet<Nature> Natures { get; set; }
        public DbSet<UOM> UOM { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Phase> Phases { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Postfix> Postfixes { get; set; }
        public DbSet<PropertyList> PropertyLists { get; set; }
        public DbSet<Finishes> Finishes { get; set; }
        public DbSet<Real_Estate> Real_Estates { get; set; }
        public DbSet<ConstructionStage> ConstructionStages { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<Prefix> Prefixes { get; set; }
        public DbSet<PropertyNo> PropertyNos { get; set; }
        public DbSet<RegistrationNo> RegistrationNos { get; set; }
        public DbSet<SocialStatus> SocialStatus { get; set; }
        public DbSet<MapDesign> MapDesigns { get; set; }
        public DbSet<GracePeriodSetup> GracePeriodSetup { get; set; }
        public DbSet<SAPOperations> SAPOperations { get; set; }
        public DbSet<SAPBilling> SAPBilling { get; set; }
        public DbSet<GLDetermination> GLDetermination { get; set; }
        public DbSet<StockCreationSetup> StockCreationSetup { get; set; }
        public DbSet<SitePlan> SitePlans { get; set; }
        public DbSet<SitePlanAttachments> SitePlanAttachments { get; set; }


        //stock 
        public DbSet<StockCreation> StockCreations { get; set; }
        public DbSet<TanantDetail> TanantDetail { get; set; }
        public DbSet<PossessionFormAttachments> PossessionFormAttachments { get; set; }

        //Users
        public DbSet<PMSUser> PMSUser { get; set; }
        public DbSet<Department> Departments { get; set; }

        //Construction 
        public DbSet<ConstructionSecurity> ConstructionSecurity { get; set; }
        public DbSet<ConstructionSecurityLabour> ConstructionSecurityLabour { get; set; }
        public DbSet<ConstructionSecurityAttachment> ConstructionSecurityAttachment { get; set; }
        public DbSet<ConstructionMonitoring> ConstructionMonitoring { get; set; }
        public DbSet<ConstructionMonitoringStageDetail> ConstructionMonitoringStageDetail { get; set; }
        public DbSet<YardStickCM> YardStickCM { get; set; }
        public DbSet<ViolationCM> ViolationCM { get; set; }
        public DbSet<StackingCM> StackingCM { get; set; }
        public DbSet<SiteServicesCM> SiteServicesCM { get; set; }
        public DbSet<MeterialTesting> MeterialTesting { get; set; }

        //Demarcation old
        public DbSet<Clearance> Clearance { get; set; }
        public DbSet<DemarcationCharge> DemarcationCharges { get; set; }

        //dev05 models
        public DbSet<DealerDesignation> DealerDesignation { get; set; }
        public DbSet<DealerCategory> DealerCategories { get; set; }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<DealerEstateDeatail> dealerEstateDeatails { get; set; }
        public DbSet<DealerAttachments> DealerAttachments { get; set; }
        public DbSet<DealerRelationshipHistery> DealerRelationshipHistery { get; set; }// ui
        public DbSet<DealerWitness> DealerWitness { get; set; }

        //Global charge setup
        public DbSet<GlobalChargeGroup> GlobalChargeGroup { get; set; }
        public DbSet<ChargeGroupType> ChargeGroupType { get; set; }
        public DbSet<GlobalChargeSetup> GlobalChargeSetup { get; set; }
        public DbSet<GlobalChargeDetail> GlobalChargeDetail { get; set; }

        //Demarcation
        public DbSet<NewDemarcationRequest> NewDemarcationRequest { get; set; }
        public DbSet<NewDemarcationRequestDetail> NewDemarcationRequestDetail { get; set; }
        public DbSet<Demarcation> Demarcation { get; set; }
        public DbSet<DemarcationFormAttachments> DemarcationFormAttachments { get; set; }

        //Approval
        public DbSet<ApprovalUI> ApprovalUI { get; set; }
        public DbSet<ApprovalSetup> ApprovalSetup { get; set; }
        public DbSet<ApprovalUsers> ApprovalUsers { get; set; }
        public DbSet<TestApproval> TestApproval { get; set; }
        public DbSet<ApprovalHistery> ApprovalHistery { get; set; } // ui

        //violation
        public DbSet<ViolationGroup> ViolationGroup { get; set; }
        public DbSet<ViolationGroupType> ViolationGroupType { get; set; }

        //Map Approval
        public DbSet<MapApprovalHistery> MapApprovalHistery { get; set; }// ui


        //Sale Module
        public DbSet<PaymentPlanSetup> PaymentPlanSetup { get; set; }
        public DbSet<PlanInformation> PlanInformation { get; set; }
        public DbSet<LeadGenration> LeadGenration { get; set; }
        public DbSet<LGSocialStatus> LGSocialStatus { get; set; }
        public DbSet<LGActivities> LGActivities { get; set; }
        public DbSet<LGInterests> LGInterests { get; set; }
        public DbSet<PreSale> PreSale { get; set; }
        public DbSet<TermsConditions> TermsConditions { get; set; }
        public DbSet<PaymentPlan> PaymentPlan { get; set; }

        //MemberProfile

        public DbSet<MemberProfile> MemberProfile { get; set; }
        public DbSet<MemberSocialStatus> MemberSocialStatus { get; set; }
        public DbSet<MemberInterest> MemberInterest { get; set; }
        public DbSet<MemberRelationshipHistery> MemberRelationshipHistery { get; set; }// ui
        public DbSet<MemberNominees> MemberNominees { get; set; }
        public DbSet<MemberAttachments> MemberAttachments { get; set; }
        public DbSet<MemberBioMetric> MemberBioMetrics { get; set; }
        public DbSet<MemberBioMetricHistery> MemberBioMetricHistery { get; set; } // ui

        //RegistrationProfile

        public DbSet<RegistrationNoProfile> RegistrationNoProfile { get; set; }
        public DbSet<SoftLock> SoftLock { get; set; }
        public DbSet<Alerts> Alerts { get; set; }
        public DbSet<RegNoProfileAttachments> RegNoProfileAttachments { get; set; }

        //Demand Note

        public DbSet<DemandNote> DemandNote { get; set; }
        public DbSet<DemandNoteItems> DemandNoteItems { get; set; }

        //Booking 

        public DbSet<Booking> Booking { get; set; }
        public DbSet<BookingProcessingCharges> BookingProcessingCharges { get; set; }
        public DbSet<BookingSchedulePaymentPlanDetail> BookingSchedulePaymentPlanDetail { get; set; }
        public DbSet<BookingJointMember> BookingJointMember { get; set; }
        public DbSet<BookingNominee> BookingNominee { get; set; }

        // Transfer

        public DbSet<TransferType> TransferType { get; set; }
        public DbSet<TaxType> TaxType { get; set; }
        public DbSet<BuyerTaxes> BuyerTaxes { get; set; }
        public DbSet<SellerTaxes> SellerTaxes { get; set; }
        public DbSet<NDCRequestType> NDCRequestType { get; set; }

        public DbSet<NDCRequestForMember> NDCRequestForMember { get; set; }
        public DbSet<NDCRequestForMemberCharges> NDCRequestForMemberCharges { get; set; }
        public DbSet<NDCRequestForMemberAttachments> NDCRequestForMemberAttachments { get; set; }

        public DbSet<NDCRequestForDealer> NDCRequestForDealer { get; set; }
        public DbSet<NDCRequestForDealerCharges> NDCRequestForDealerCharges { get; set; }
        public DbSet<NDCRequestForDealerAttachments> NDCRequestForDealerAttachments { get; set; }

        public DbSet<NDC1> NDC1 { get; set; }
        public DbSet<NDC1PowerOfAttorey> NDC1PowerOfAttorey { get; set; }
        public DbSet<NDC1Attachments> NDC1Attachments { get; set; }
        public DbSet<NDC1CheckList> NDC1CheckLists { get; set; }

        //Role Based Permissions
        public DbSet<RolesPermissions> RolesPermissions { get; set; }
        public DbSet<Permissions> Permissions { get; set; }

        // TransferHistery
        public DbSet<TransferHistery> TransferHistery { get; set; } // ui
        public DbSet<TransferHisteryJointMember> TransferHisteryJointMember { get; set; } // ui
        public DbSet<TransferHisteryNominee> TransferHisteryNominee { get; set; } //ui
        public DbSet<TransferHisteryAttachments> TransferHisteryAttachments { get; set; } //ui

        // Deal
        public DbSet<Deal> Deal { get; set; }
        public DbSet<DealProperty> DealProperty { get; set; }
        public DbSet<DealPaymentPlan> DealPaymentPlan { get; set; }

        // Mobile APIs
        public DbSet<Promotion> Promotion { get; set; }
        public DbSet<Banner> Banner { get; set; }

        //PaymentPlan
        public DbSet<PaymentPlanType> PaymentPlanType { get; set; }

        //FormsChargeGroup
        public DbSet<FormsChargeGroup> FormsChargeGroup { get; set; }


        //Advance Application
        public DbSet<AdvanceApplication> AdvanceApplication { get; set; }
        public DbSet<DealAdvanceApplicationHistery> DealAdvanceApplicationHistery { get; set; } // ui
        public DbSet<DealAdvanceApplicationRecipt> DealAdvanceApplicationRecipt { get; set; }

        // Surrender

        public DbSet<Surrender> Surrender { get; set; }
        public DbSet<SurrenderHistery> SurrenderHistery { get; set; } //ui
        // public DbSet<ResurrenderCharges> ResurrenderCharges { get; set; }

        // Billing

        public DbSet<MeterType> MeterType { get; set; }
        public DbSet<MeterPhase> MeterPhase { get; set; }
        public DbSet<MeterPhaseWiseRate> MeterPhaseWiseRates { get; set; }
        public DbSet<MeterStatus> MeterStatus { get; set; }
        public DbSet<ReadingOfficer> ReadingOfficer { get; set; }
        public DbSet<MeterInstallation> MeterInstallation { get; set; }
        public DbSet<MeterDetail> MeterDetail { get; set; }
        public DbSet<MeterReading> MeterReading { get; set; }
        public DbSet<ReadingDetail> ReadingDetail { get; set; }
        public DbSet<MeterBillGeneration> MeterBillGeneration { get; set; }
        public DbSet<MeterBillGenerationDetail> MeterBillGenerationDetail { get; set; }
        public DbSet<IndividualBill> IndividualBill { get; set; }
        public DbSet<IndividualBillDetail> IndividualBillDetail { get; set; }
        public DbSet<FixedChargeBill> FixedChargeBill { get; set; }
        public DbSet<FixedChargeBillDetail> FixedChargeBillDetail { get; set; }
        public DbSet<WithHoldingTax> WithHoldingTax { get; set; }
        public DbSet<SaleTax> SaleTax { get; set; }
        public DbSet<WithHoldingTaxPropertyWise> WithHoldingTaxPropertyWise { get; set; }
        public DbSet<FixedChargesEnabledSetupPropertyWise> FixedChargesEnableSetupPropertyWise { get; set; }
        public DbSet<PropertyFixedChargesSetup> PropertyFixedChargesSetup { get; set; }
        public DbSet<FixedChargeBillWHApplied> FixedChargeBillWHApplied { get; set; }
        public DbSet<SAPBillPostingCheck> SAPBillPostingCheck { get; set; }
        public DbSet<BillingServiceDetailsTable> BillingServiceDetailsTable { get; set; }
        public DbSet<BillingServiceTable> BillingServiceTable { get; set; }

        // UserBase Permissions

        public DbSet<PermissionForms> PermissionForms { get; set; }
        public DbSet<UserPermissionMapping> UserPermissionMapping { get; set; }

        // Form / navigation registry (Instructions §5) — table is created by
        // NavigationRegistrySeeder at startup until the migration squash lands.
        public DbSet<NavigationNode> NavigationNodes { get; set; }

        //Litigation

        public DbSet<CaseCategory> CaseCategory { get; set; }
        public DbSet<CaseType> CaseType { get; set; }
        public DbSet<Forum> Forum { get; set; }
        public DbSet<LawyerData> LawyerData { get; set; }
        public DbSet<CaseProfile> CaseProfile { get; set; }
        public DbSet<CaseProfileParties> CaseProfileParties { get; set; }
        public DbSet<CaseProfileCaseHearings> CaseProfileCaseHearings { get; set; }
        public DbSet<CaseProfileAppeals> CaseProfileAppeals { get; set; }
        public DbSet<CaseProfileNotices> CaseProfileNotices { get; set; }
        public DbSet<CaseProfileAttachments> CaseProfileAttachments { get; set; }

        // Calendar setup
        public DbSet<WeekSchedule> WeekSchedules { get; set; }
        public DbSet<WeekScheduleExective> WeekScheduleExective { get; set; }

        // Transfer Receipt setup
        public DbSet<TransferReceiptProcessing> TransferReceiptProcessing { get; set; }
        public DbSet<GovtBuyerCharges> GovtBuyerCharges { get; set; }
        public DbSet<GovtSellerCharges> GovtSellerCharges { get; set; }
        public DbSet<TransferAttachments> TransferAttachments { get; set; }
        public DbSet<TransferReceiptJointMember> TransferReceiptJointMember { get; set; }
        public DbSet<TransferReceiptNominee> TransferReceiptNominee { get; set; }

        //Alerts
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationReceiver> NotificationReceivers { get; set; }
        public DbSet<FormAlert> FormAlerts { get; set; }
        public DbSet<FormAlertUsers> FormAlertUsers { get; set; }
        public DbSet<AlertName> AlertNames { get; set; }
        public DbSet<SoftLockName> SoftLockNames { get; set; }

        //Repurchase
        public DbSet<RePurchase> RePurchase { get; set; }
        public DbSet<RePurchaseFinanceDetail> RePurchaseFinanceDetails { get; set; }
        public DbSet<RePurchasePropertyDivision> RePurchasePropertyDivisions { get; set; }
        public DbSet<COPHistery> COPHistories { get; set; } // ui
        public DbSet<RenumberHistery> RenumberHistories { get; set; } // ui

        // FileVerification

        public DbSet<FileVerificationRequest> FileVerificationRequests { get; set; }
        public DbSet<FileVerificationRequestCharges> FileVerificationRequestCharges { get; set; }
        public DbSet<FileVerificationAttachments> FileVerificationAttachments { get; set; }
        public DbSet<FileVerificationNDC1> FileVerificationNDC1 { get; set; }
        public DbSet<FileVerificationNDC1Attachments> FileVerificationNDC1Attachments { get; set; }
        public DbSet<FileVerificationNDC1CheckList> FileVerificationNDC1CheckList { get; set; }
        public DbSet<FileVerificationNDC1PowerOfAttorey> FileVerificationNDC1PowerOfAttorey { get; set; }
        public DbSet<ClientFileVerification> ClientFileVerification { get; set; }
        public DbSet<ClientFileVerificationAttachments> ClientFileVerificationAttachments { get; set; }
        public DbSet<FileDocDupRequest> FileDocDupRequests { get; set; }
        public DbSet<FileDocDupRequestedCharges> FileDocDupRequestedCharges { get; set; }

        //Bulk Deals
        public DbSet<BulkDeal> BulkDeal { get; set; }
        public DbSet<BulkDealProposePlan> BulkDealProposePlan { get; set; }
        public DbSet<BulkDealProperty> BulkDealProperty { get; set; }
        public DbSet<BulkPaymentSchedule> BulkPaymentSchedule { get; set; }

        //Store Room
        public DbSet<StoreRoomFileMoving> StoreRoomFileMoving { get; set; }
        public DbSet<FileLocationAssigment> FileLocationAssigments { get; set; }

        // File Receiving Register
          public DbSet<FileReceivingRegister> FileReceivingRegisters { get; set; }

        // GenralAdjustment

        public DbSet<GenralAdjustment> GenralAdjustments { get; set; }
        public DbSet<GenralAdjustmentCharges> GenralAdjustmentCharges { get; set; }
        public DbSet<StandAlone> StandAlones { get; set; }
        public DbSet<StandAloneCharges> StandAloneCharges { get; set; }

        // For Sp
        public DbSet<JsonOutPutModel> JsonOutPutModel { get; set; }


        //ForBookingBackLog
        public DbSet<BookingBackLog> BookingBackLog { get; set; }

        //TransferSetReceiving
        public DbSet<TransferSetReceiving> TransferSetReceivings { get; set; }
        public DbSet<TransferSetReceivingAttachments> TransferSetReceivingAttachments { get; set; }


        public DbSet<PropertyStatus> PropertyStatus { get; set; }

        public DbSet<JointMemberHistoricalData> JointMemberHistoricalData { get; set; }
        public DbSet<TransferHistoricalData> TransferHistoricalData { get; set; }
        public DbSet<VoucherSeries> VoucherSeries { get; set; }

        public DbSet<PossessionAttachment> PossessionAttachments { get; set; }

        #region Dynamic Queries
        public DbSet<DynamicQuery> DynamicQueries { get; set; }
        public DbSet<QueryParam> QueryParams { get; set; }
        public DbSet<QueryParamOption> QueryParamOptions { get; set; }

        #endregion

        #region Amalgamation
        public DbSet<Amalgamation> Amalgamation { get; set; }
        public DbSet<AmalgamationDetails> AmalgamationDetails { get; set; }
        #endregion

        public DbSet<CredentialConfig> CredentialConfig { get; set; }
        public DbSet<Inovice> Inovices { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PreSale>().ToTable(name:"PreSale", t=> t.IsTemporal());
            modelBuilder.Entity<Block>().ToTable(name: "Block", t => t.IsTemporal());
            modelBuilder.Entity<Force>().ToTable(name: "Force", t => t.IsTemporal());
            modelBuilder.Entity<Rank>().ToTable(name: "Rank", t => t.IsTemporal());
            modelBuilder.Entity<Sector>().ToTable(name: "Sector", t => t.IsTemporal());
            modelBuilder.Entity<Quota>().ToTable(name: "Quota", t => t.IsTemporal());
            modelBuilder.Entity<Almt>().ToTable(name: "Almt", t => t.IsTemporal());
            modelBuilder.Entity<MemberCategory>().ToTable(name: "MemberCategory", t => t.IsTemporal());
            modelBuilder.Entity<PropertyList>().ToTable(name: "PropertyList", t => t.IsTemporal());
            modelBuilder.Entity<Postfix>().ToTable(name: "Postfix", t => t.IsTemporal());
            modelBuilder.Entity<Project>().ToTable(name: "Project", t => t.IsTemporal());
            modelBuilder.Entity<Floor>().ToTable(name: "Floor", t => t.IsTemporal());
            modelBuilder.Entity<Phase>().ToTable(name: "Phase", t => t.IsTemporal());
            modelBuilder.Entity<Feature>().ToTable(name: "Feature", t => t.IsTemporal());
            modelBuilder.Entity<UOM>().ToTable(name: "UOM", t => t.IsTemporal());
            modelBuilder.Entity<Nature>().ToTable(name: "Nature", t => t.IsTemporal());
            modelBuilder.Entity<SurchargeSetup>().ToTable(name: "SurchargeSetup", t => t.IsTemporal());
            modelBuilder.Entity<Category>().ToTable(name: "Category", t => t.IsTemporal());
            modelBuilder.Entity<SitePlanAttachments>().ToTable(name: "SitePlanAttachments", t => t.IsTemporal());
            modelBuilder.Entity<SitePlan>().ToTable(name: "SitePlan", t => t.IsTemporal());
            modelBuilder.Entity<StockCreationSetup>().ToTable(name: "StockCreationSetup", t => t.IsTemporal());
            modelBuilder.Entity<GLDetermination>().ToTable(name: "GLDetermination", t => t.IsTemporal());
            modelBuilder.Entity<SAPBilling>().ToTable(name: "SAPBilling", t => t.IsTemporal());
            modelBuilder.Entity<SAPOperations>().ToTable(name: "SAPOperations", t => t.IsTemporal());
            modelBuilder.Entity<GracePeriodSetup>().ToTable(name: "GracePeriodSetup", t => t.IsTemporal());
            modelBuilder.Entity<MapDesign>().ToTable(name: "MapDesign", t => t.IsTemporal());
            modelBuilder.Entity<SocialStatus>().ToTable(name: "SocialStatus", t => t.IsTemporal());
            modelBuilder.Entity<RegistrationNo>().ToTable(name: "RegistrationNo", t => t.IsTemporal());
            modelBuilder.Entity<PropertyNo>().ToTable(name: "PropertyNo", t => t.IsTemporal());
            modelBuilder.Entity<Prefix>().ToTable(name: "Prefix", t => t.IsTemporal());
            modelBuilder.Entity<PropertyType>().ToTable(name: "PropertyType", t => t.IsTemporal());
            modelBuilder.Entity<ConstructionStage>().ToTable(name: "ConstructionStage", t => t.IsTemporal());
            modelBuilder.Entity<Real_Estate>().ToTable(name: "Real_Estate", t => t.IsTemporal());
            modelBuilder.Entity<Finishes>().ToTable(name: "Finishes", t => t.IsTemporal());
            modelBuilder.Entity<DealerWitness>().ToTable(name: "DealerWitness", t => t.IsTemporal());
            modelBuilder.Entity<DealerRelationshipHistery>().ToTable(name: "DealerRelationshipHistery", t => t.IsTemporal());
            modelBuilder.Entity<DealerAttachments>().ToTable(name: "DealerAttachments", t => t.IsTemporal());
            modelBuilder.Entity<DealerEstateDeatail>().ToTable(name: "DealerEstateDeatail", t => t.IsTemporal());
            modelBuilder.Entity<Dealer>().ToTable(name: "Dealer", t => t.IsTemporal());
            modelBuilder.Entity<DealerCategory>().ToTable(name: "DealerCategory", t => t.IsTemporal());
            modelBuilder.Entity<DealerDesignation>().ToTable(name: "DealerDesignation", t => t.IsTemporal());
            modelBuilder.Entity<DemarcationCharge>().ToTable(name: "DemarcationCharge", t => t.IsTemporal());
            modelBuilder.Entity<Clearance>().ToTable(name: "Clearance", t => t.IsTemporal());
            modelBuilder.Entity<ConstructionMonitoringStageDetail>().ToTable(name: "ConstructionMonitoringStageDetail", t => t.IsTemporal());
            modelBuilder.Entity<ConstructionMonitoring>().ToTable(name: "ConstructionMonitoring", t => t.IsTemporal());
            modelBuilder.Entity<ConstructionSecurityAttachment>().ToTable(name: "ConstructionSecurityAttachment", t => t.IsTemporal());
            modelBuilder.Entity<ConstructionSecurityLabour>().ToTable(name: "ConstructionSecurityLabour", t => t.IsTemporal());
            modelBuilder.Entity<ConstructionSecurity>().ToTable(name: "ConstructionSecurity", t => t.IsTemporal());
            modelBuilder.Entity<Department>().ToTable(name: "Department", t => t.IsTemporal());
            modelBuilder.Entity<PMSUser>().ToTable(name: "PMSUser", t => t.IsTemporal());
            modelBuilder.Entity<TanantDetail>().ToTable(name: "TanantDetail", t => t.IsTemporal());
            modelBuilder.Entity<StockCreation>().ToTable(name: "StockCreation", t => t.IsTemporal());
            modelBuilder.Entity<MapApprovalHistery>().ToTable(name: "MapApprovalHistery", t => t.IsTemporal());
            modelBuilder.Entity<ViolationGroupType>().ToTable(name: "ViolationGroupType", t => t.IsTemporal());
            modelBuilder.Entity<ViolationGroup>().ToTable(name: "ViolationGroup", t => t.IsTemporal());
            modelBuilder.Entity<ApprovalHistery>().ToTable(name: "ApprovalHistery", t => t.IsTemporal());
            modelBuilder.Entity<TestApproval>().ToTable(name: "TestApproval", t => t.IsTemporal());
            modelBuilder.Entity<ApprovalUsers>().ToTable(name: "ApprovalUsers", t => t.IsTemporal());
            modelBuilder.Entity<ApprovalSetup>().ToTable(name: "ApprovalSetup", t => t.IsTemporal());
            modelBuilder.Entity<ApprovalUI>().ToTable(name: "ApprovalUI", t => t.IsTemporal());
            modelBuilder.Entity<DemarcationFormAttachments>().ToTable(name: "DemarcationFormAttachments", t => t.IsTemporal());
            modelBuilder.Entity<Demarcation>().ToTable(name: "Demarcation", t => t.IsTemporal());
            modelBuilder.Entity<NewDemarcationRequestDetail>().ToTable(name: "NewDemarcationRequestDetail", t => t.IsTemporal());
            modelBuilder.Entity<NewDemarcationRequest>().ToTable(name: "NewDemarcationRequest", t => t.IsTemporal());
            modelBuilder.Entity<GlobalChargeDetail>().ToTable(name: "GlobalChargeDetail", t => t.IsTemporal());
            modelBuilder.Entity<GlobalChargeSetup>().ToTable(name: "GlobalChargeSetup", t => t.IsTemporal());
            modelBuilder.Entity<ChargeGroupType>().ToTable(name: "ChargeGroupType", t => t.IsTemporal());
            modelBuilder.Entity<GlobalChargeGroup>().ToTable(name: "GlobalChargeGroup", t => t.IsTemporal());
            modelBuilder.Entity<DemandNoteItems>().ToTable(name: "DemandNoteItems", t => t.IsTemporal());
            modelBuilder.Entity<DemandNote>().ToTable(name: "DemandNote", t => t.IsTemporal());
            modelBuilder.Entity<RegNoProfileAttachments>().ToTable(name: "RegNoProfileAttachments", t => t.IsTemporal());
            modelBuilder.Entity<Alerts>().ToTable(name: "Alerts", t => t.IsTemporal());
            modelBuilder.Entity<SoftLock>().ToTable(name: "SoftLock", t => t.IsTemporal());
            modelBuilder.Entity<RegistrationNoProfile>().ToTable(name: "RegistrationNoProfile", t => t.IsTemporal());
            modelBuilder.Entity<MemberBioMetricHistery>().ToTable(name: "MemberBioMetricHistery", t => t.IsTemporal());
            modelBuilder.Entity<MemberBioMetric>().ToTable(name: "MemberBioMetric", t => t.IsTemporal());
            modelBuilder.Entity<MemberAttachments>().ToTable(name: "MemberAttachments", t => t.IsTemporal());
            modelBuilder.Entity<MemberNominees>().ToTable(name: "MemberNominees", t => t.IsTemporal());
            modelBuilder.Entity<MemberRelationshipHistery>().ToTable(name: "MemberRelationshipHistery", t => t.IsTemporal());
            modelBuilder.Entity<MemberInterest>().ToTable(name: "MemberInterest", t => t.IsTemporal());
            modelBuilder.Entity<MemberSocialStatus>().ToTable(name: "MemberSocialStatus", t => t.IsTemporal());
            modelBuilder.Entity<MemberProfile>().ToTable(name: "MemberProfile", t => t.IsTemporal());
            modelBuilder.Entity<PaymentPlan>().ToTable(name: "PaymentPlan", t => t.IsTemporal());
            modelBuilder.Entity<TermsConditions>().ToTable(name: "TermsConditions", t => t.IsTemporal());
            modelBuilder.Entity<LGInterests>().ToTable(name: "LGInterests", t => t.IsTemporal());
            modelBuilder.Entity<LGActivities>().ToTable(name: "LGActivities", t => t.IsTemporal());
            modelBuilder.Entity<LGSocialStatus>().ToTable(name: "LGSocialStatus", t => t.IsTemporal());
            modelBuilder.Entity<LeadGenration>().ToTable(name: "LeadGenration", t => t.IsTemporal());
            modelBuilder.Entity<PlanInformation>().ToTable(name: "PlanInformation", t => t.IsTemporal());
            modelBuilder.Entity<PaymentPlanSetup>().ToTable(name: "PaymentPlanSetup", t => t.IsTemporal());
            modelBuilder.Entity<Permissions>().ToTable(name: "Permissions", t => t.IsTemporal());
            modelBuilder.Entity<RolesPermissions>().ToTable(name: "RolesPermissions", t => t.IsTemporal());
            modelBuilder.Entity<NDC1CheckList>().ToTable(name: "NDC1CheckList", t => t.IsTemporal());
            modelBuilder.Entity<NDC1Attachments>().ToTable(name: "NDC1Attachments", t => t.IsTemporal());
            modelBuilder.Entity<NDC1PowerOfAttorey>().ToTable(name: "NDC1PowerOfAttorey", t => t.IsTemporal());
            modelBuilder.Entity<NDC1>().ToTable(name: "NDC1", t => t.IsTemporal());
            modelBuilder.Entity<NDCRequestForDealerAttachments>().ToTable(name: "NDCRequestForDealerAttachments", t => t.IsTemporal());
            modelBuilder.Entity<NDCRequestForDealerCharges>().ToTable(name: "NDCRequestForDealerCharges", t => t.IsTemporal());
            modelBuilder.Entity<NDCRequestForDealer>().ToTable(name: "NDCRequestForDealer", t => t.IsTemporal());
            modelBuilder.Entity<NDCRequestForMemberAttachments>().ToTable(name: "NDCRequestForMemberAttachments", t => t.IsTemporal());
            modelBuilder.Entity<NDCRequestForMemberCharges>().ToTable(name: "NDCRequestForMemberCharges", t => t.IsTemporal());
            modelBuilder.Entity<NDCRequestForMember>().ToTable(name: "NDCRequestForMember", t => t.IsTemporal());
            modelBuilder.Entity<NDCRequestType>().ToTable(name: "NDCRequestType", t => t.IsTemporal());
            modelBuilder.Entity<TransferType>().ToTable(name: "TransferType", t => t.IsTemporal());
            modelBuilder.Entity<TaxType>().ToTable(name: "TaxType", t => t.IsTemporal());
            modelBuilder.Entity<SellerTaxes>().ToTable(name: "SellerTaxes", t => t.IsTemporal());
            modelBuilder.Entity<BuyerTaxes>().ToTable(name: "BuyerTaxes", t => t.IsTemporal());
            modelBuilder.Entity<BookingNominee>().ToTable(name: "BookingNominee", t => t.IsTemporal());
            modelBuilder.Entity<BookingJointMember>().ToTable(name: "BookingJointMember", t => t.IsTemporal());
            modelBuilder.Entity<BookingSchedulePaymentPlanDetail>().ToTable(name: "BookingSchedulePaymentPlanDetail", t => t.IsTemporal());
            modelBuilder.Entity<BookingProcessingCharges>().ToTable(name: "BookingProcessingCharges", t => t.IsTemporal());
            modelBuilder.Entity<Booking>().ToTable(name: "Booking", t => t.IsTemporal());
            modelBuilder.Entity<ResurrenderCharges>().ToTable(name: "ResurrenderCharges", t => t.IsTemporal());
            modelBuilder.Entity<SurrenderHistery>().ToTable(name: "SurrenderHistery", t => t.IsTemporal());
            modelBuilder.Entity<Surrender>().ToTable(name: "Surrender", t => t.IsTemporal());
            modelBuilder.Entity<DealAdvanceApplicationHistery>().ToTable(name: "DealAdvanceApplicationHistery", t => t.IsTemporal());
            modelBuilder.Entity<DealAdvanceApplicationRecipt>().ToTable(name: "DealAdvanceApplicationRecipt", t => t.IsTemporal());
            modelBuilder.Entity<AdvanceApplication>().ToTable(name: "AdvanceApplication", t => t.IsTemporal());
            modelBuilder.Entity<FormsChargeGroup>().ToTable(name: "FormsChargeGroup", t => t.IsTemporal());
            modelBuilder.Entity<PaymentPlanType>().ToTable(name: "PaymentPlanType", t => t.IsTemporal());
            modelBuilder.Entity<Banner>().ToTable(name: "Banner", t => t.IsTemporal());
            modelBuilder.Entity<Promotion>().ToTable(name: "Promotion", t => t.IsTemporal());
            modelBuilder.Entity<DealPaymentPlan>().ToTable(name: "DealPaymentPlan", t => t.IsTemporal());
            modelBuilder.Entity<DealProperty>().ToTable(name: "DealProperty", t => t.IsTemporal());
            modelBuilder.Entity<Deal>().ToTable(name: "Deal", t => t.IsTemporal());
            modelBuilder.Entity<TransferHisteryAttachments>().ToTable(name: "TransferHisteryAttachments", t => t.IsTemporal());
            modelBuilder.Entity<TransferHisteryNominee>().ToTable(name: "TransferHisteryNominee", t => t.IsTemporal());
            modelBuilder.Entity<TransferHisteryJointMember>().ToTable(name: "TransferHisteryJointMember", t => t.IsTemporal());
            modelBuilder.Entity<TransferHistery>().ToTable(name: "TransferHistery", t => t.IsTemporal());
            modelBuilder.Entity<SAPBillPostingCheck>().ToTable(name: "SAPBillPostingCheck", t => t.IsTemporal());
            modelBuilder.Entity<FixedChargeBillWHApplied>().ToTable(name: "FixedChargeBillWHApplied", t => t.IsTemporal());
            modelBuilder.Entity<PropertyFixedChargesSetup>().ToTable(name: "PropertyFixedChargesSetup", t => t.IsTemporal());
            modelBuilder.Entity<FixedChargesEnabledSetupPropertyWise>().ToTable(name: "FixedChargesEnabledSetupPropertyWise", t => t.IsTemporal());
            modelBuilder.Entity<WithHoldingTaxPropertyWise>().ToTable(name: "WithHoldingTaxPropertyWise", t => t.IsTemporal());
            modelBuilder.Entity<SaleTax>().ToTable(name: "SaleTax", t => t.IsTemporal());
            modelBuilder.Entity<WithHoldingTax>().ToTable(name: "WithHoldingTax", t => t.IsTemporal());
            modelBuilder.Entity<FixedChargeBillDetail>().ToTable(name: "FixedChargeBillDetail", t => t.IsTemporal());
            modelBuilder.Entity<FixedChargeBill>().ToTable(name: "FixedChargeBill", t => t.IsTemporal());
            modelBuilder.Entity<IndividualBillDetail>().ToTable(name: "IndividualBillDetail", t => t.IsTemporal());
            modelBuilder.Entity<IndividualBill>().ToTable(name: "IndividualBill", t => t.IsTemporal());
            modelBuilder.Entity<MeterBillGenerationDetail>().ToTable(name: "MeterBillGenerationDetail", t => t.IsTemporal());
            modelBuilder.Entity<MeterBillGeneration>().ToTable(name: "MeterBillGeneration", t => t.IsTemporal());
            modelBuilder.Entity<ReadingDetail>().ToTable(name: "ReadingDetail", t => t.IsTemporal());
            modelBuilder.Entity<MeterReading>().ToTable(name: "MeterReading", t => t.IsTemporal());
            modelBuilder.Entity<MeterDetail>().ToTable(name: "MeterDetail", t => t.IsTemporal());
            modelBuilder.Entity<MeterInstallation>().ToTable(name: "MeterInstallation", t => t.IsTemporal());
            modelBuilder.Entity<ReadingOfficer>().ToTable(name: "ReadingOfficer", t => t.IsTemporal());
            modelBuilder.Entity<MeterPhaseWiseRate>().ToTable(name: "MeterPhaseWiseRate", t => t.IsTemporal());
            modelBuilder.Entity<MeterStatus>().ToTable(name: "MeterStatus", t => t.IsTemporal());
            modelBuilder.Entity<MeterPhase>().ToTable(name: "MeterPhase", t => t.IsTemporal());
            modelBuilder.Entity<MeterType>().ToTable(name: "MeterType", t => t.IsTemporal());
            modelBuilder.Entity<TransferReceiptProcessing>().ToTable(name: "TransferReceiptProcessing", t => t.IsTemporal());
            modelBuilder.Entity<WeekScheduleExective>().ToTable(name: "WeekScheduleExective", t => t.IsTemporal());
            modelBuilder.Entity<WeekSchedule>().ToTable(name: "WeekSchedule", t => t.IsTemporal());
            modelBuilder.Entity<CaseProfileAttachments>().ToTable(name: "CaseProfileAttachments", t => t.IsTemporal());
            modelBuilder.Entity<CaseProfileNotices>().ToTable(name: "CaseProfileNotices", t => t.IsTemporal());
            modelBuilder.Entity<CaseProfileAppeals>().ToTable(name: "CaseProfileAppeals", t => t.IsTemporal());
            modelBuilder.Entity<CaseProfileCaseHearings>().ToTable(name: "CaseProfileCaseHearings", t => t.IsTemporal());
            modelBuilder.Entity<CaseProfileParties>().ToTable(name: "CaseProfileParties", t => t.IsTemporal());
            modelBuilder.Entity<CaseProfile>().ToTable(name: "CaseProfile", t => t.IsTemporal());
            modelBuilder.Entity<LawyerData>().ToTable(name: "LawyerData", t => t.IsTemporal());
            modelBuilder.Entity<Forum>().ToTable(name: "Forum", t => t.IsTemporal());
            modelBuilder.Entity<CaseType>().ToTable(name: "CaseType", t => t.IsTemporal());
            modelBuilder.Entity<CaseCategory>().ToTable(name: "CaseCategory", t => t.IsTemporal());
            modelBuilder.Entity<UserPermissionMapping>().ToTable(name: "UserPermissionMapping", t => t.IsTemporal());
            modelBuilder.Entity<PermissionForms>().ToTable(name: "PermissionForms", t => t.IsTemporal());
            modelBuilder.Entity<FileVerificationNDC1>().ToTable(name: "FileVerificationNDC1", t => t.IsTemporal());
            modelBuilder.Entity<FileVerificationAttachments>().ToTable(name: "FileVerificationAttachments", t => t.IsTemporal());
            modelBuilder.Entity<FileVerificationRequestCharges>().ToTable(name: "FileVerificationRequestCharges", t => t.IsTemporal());
            modelBuilder.Entity<FileVerificationRequest>().ToTable(name: "FileVerificationRequest", t => t.IsTemporal());
            modelBuilder.Entity<RenumberHistery>().ToTable(name: "RenumberHistery", t => t.IsTemporal());
            modelBuilder.Entity<COPHistery>().ToTable(name: "COPHistery", t => t.IsTemporal());
            modelBuilder.Entity<RePurchasePropertyDivision>().ToTable(name: "RePurchasePropertyDivision", t => t.IsTemporal());
            modelBuilder.Entity<RePurchaseFinanceDetail>().ToTable(name: "RePurchaseFinanceDetail", t => t.IsTemporal());
            modelBuilder.Entity<RePurchase>().ToTable(name: "RePurchase", t => t.IsTemporal());
            modelBuilder.Entity<SoftLockName>().ToTable(name: "SoftLockName", t => t.IsTemporal());
            modelBuilder.Entity<FormAlertUsers>().ToTable(name: "FormAlertUsers", t => t.IsTemporal());
            modelBuilder.Entity<AlertName>().ToTable(name: "AlertName", t => t.IsTemporal());
            modelBuilder.Entity<NotificationReceiver>().ToTable(name: "NotificationReceiver", t => t.IsTemporal());
            modelBuilder.Entity<Notification>().ToTable(name: "Notification", t => t.IsTemporal());
            modelBuilder.Entity<TransferAttachments>().ToTable(name: "TransferAttachments", t => t.IsTemporal());
            modelBuilder.Entity<GovtSellerCharges>().ToTable(name: "GovtSellerCharges", t => t.IsTemporal());
            modelBuilder.Entity<GovtBuyerCharges>().ToTable(name: "GovtBuyerCharges", t => t.IsTemporal());
            modelBuilder.Entity<FileReceivingRegister>().ToTable(name: "FileReceivingRegister", t => t.IsTemporal());
            modelBuilder.Entity<FileLocationAssigment>().ToTable(name: "FileLocationAssigment", t => t.IsTemporal());
            modelBuilder.Entity<StoreRoomFileMoving>().ToTable(name: "StoreRoomFileMoving", t => t.IsTemporal());
            modelBuilder.Entity<BulkPaymentSchedule>().ToTable(name: "BulkPaymentSchedule", t => t.IsTemporal());
            modelBuilder.Entity<BulkDealProperty>().ToTable(name: "BulkDealProperty", t => t.IsTemporal());
            modelBuilder.Entity<BulkDealProposePlan>().ToTable(name: "BulkDealProposePlan", t => t.IsTemporal());
            modelBuilder.Entity<BulkDeal>().ToTable(name: "BulkDeal", t => t.IsTemporal());
            modelBuilder.Entity<FileDocDupRequestedCharges>().ToTable(name: "FileDocDupRequestedCharges", t => t.IsTemporal());
            modelBuilder.Entity<FileDocDupRequest>().ToTable(name: "FileDocDupRequest", t => t.IsTemporal());
            modelBuilder.Entity<ClientFileVerificationAttachments>().ToTable(name: "ClientFileVerificationAttachments", t => t.IsTemporal());
            modelBuilder.Entity<ClientFileVerification>().ToTable(name: "ClientFileVerification", t => t.IsTemporal());
            modelBuilder.Entity<FileVerificationNDC1PowerOfAttorey>().ToTable(name: "FileVerificationNDC1PowerOfAttorey", t => t.IsTemporal());
            modelBuilder.Entity<FileVerificationNDC1CheckList>().ToTable(name: "FileVerificationNDC1CheckList", t => t.IsTemporal());
            modelBuilder.Entity<FileVerificationNDC1Attachments>().ToTable(name: "FileVerificationNDC1Attachments", t => t.IsTemporal());
            modelBuilder.Entity<TransferSetReceivingAttachments>().ToTable(name: "TransferSetReceivingAttachments", t => t.IsTemporal());
            modelBuilder.Entity<TransferSetReceiving>().ToTable(name: "TransferSetReceiving", t => t.IsTemporal());
            modelBuilder.Entity<BookingBackLog>().ToTable(name: "BookingBackLog", t => t.IsTemporal());
            modelBuilder.Entity<StandAloneCharges>().ToTable(name: "StandAloneCharges", t => t.IsTemporal());
            modelBuilder.Entity<StandAlone>().ToTable(name: "StandAlone", t => t.IsTemporal());
            modelBuilder.Entity<GenralAdjustmentCharges>().ToTable(name: "GenralAdjustmentCharges", t => t.IsTemporal());
            modelBuilder.Entity<GenralAdjustment>().ToTable(name: "GenralAdjustment", t => t.IsTemporal());
            modelBuilder.Entity<PropertyStatus>().ToTable(name: "PropertyStatus", t => t.IsTemporal());
            modelBuilder.Entity<TransferReceiptJointMember>().ToTable(name: "TransferReceiptJointMember", t => t.IsTemporal());
            modelBuilder.Entity<TransferReceiptNominee>().ToTable(name: "TransferReceiptNominee", t => t.IsTemporal());
            modelBuilder.Entity<CredentialConfig>().ToTable(name: "CredentialConfig", t => t.IsTemporal());

            modelBuilder.Entity<JsonOutPutModel>().HasNoKey().ToTable(nameof(JsonOutPutModel), t =>
                t.ExcludeFromMigrations());
        }
    }
}
