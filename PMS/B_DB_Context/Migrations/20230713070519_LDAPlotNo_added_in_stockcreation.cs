using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class LDAPlotNo_added_in_stockcreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "WithHoldingTaxPropertyWise");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "WithHoldingTax");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "WeekSchedules");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "WeekScheduleExective");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "ViolationGroupType");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "ViolationGroup");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "UserPermissionMapping");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "TransferHistoryNominee");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "TransferHistoryJointMember");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "TransferHistory");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "TestApproval");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "TanantDetail");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "SurrenderHistory");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "Surrender");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "StoreRoomFileMoving");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "StockCreationSetup");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "SoftLock");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "SAPOperations");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "SAPBillPostingCheck");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "SAPBilling");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "SaleTax");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "RolesPermissions");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "RePurchase");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "RegNoProfileAttachments");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "RegistrationNoProfile");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "ReadingOfficer");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "ReadingDetail");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "PropertyFixedChargesSetup");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "Promotion");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "PreSale");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "PlanInformation");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "PermissionForms");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "PaymentPlanSetup");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "PaymentPlan");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "NewDemarcationRequestDetail");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "NewDemarcationRequest");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "NDCRequestForMemberCharges");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "NDCRequestForMemberAttachments");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "NDCRequestForDealerCharges");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "NDCRequestForDealerAttachments");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "NDCRequestForDealer");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "NDC1PowerOfAttorey");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "NDC1CheckLists");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "NDC1Attachments");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "NDC1");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "MeterType");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "MeterStatus");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "MeterReading");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "MeterInstallation");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "MeterDetail");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "MeterBillGenerationDetail");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "MeterBillGeneration");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "MemberSocialStatus");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "MemberRelationshipHistory");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "MemberProfile");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "MemberNominees");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "MemberInterest");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "MemberAttachments");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "MapApprovalHistory");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "LeadGenration");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "LawyerData");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "IndividualBillDetail");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "IndividualBill");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "GracePeriodSetup");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "GlobalChargeSetup");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "GlobalChargeGroup");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "GlobalChargeDetail");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "GLDetermination");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "Forum");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "FormsChargeGroup");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "FormAlertUsers");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "FormAlerts");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "FixedChargesEnableSetupPropertyWise");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "FixedChargeBillWHApplied");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "FixedChargeBillDetail");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "FixedChargeBill");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "FileVerificationRequests");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "FileVerificationNDC1");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "Demarcation");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "DemandNoteItems");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "DemandNote");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "DealProperty");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "DealPaymentPlan");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "DealerWitness");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "DealerRelationshipHistory");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "dealerEstateDeatails");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "DealerCategories");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "DealerAttachments");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "DealAdvanceApplicationRecipt");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "DealAdvanceApplicationHistory");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "Deal");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "ConstructionSecurityLabour");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "ConstructionSecurityAttachment");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "ConstructionSecurity");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "ConstructionMonitoringStageDetail");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "ConstructionMonitoring");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "ClientFileVerification");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "ChargeGroupType");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "CaseType");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "CaseProfileParties");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "CaseProfileNotices");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "CaseProfileCaseHearings");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "CaseProfileAttachments");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "CaseProfileAppeals");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "CaseProfile");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "CaseCategory");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "BulkPaymentSchedule");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "BulkDealProposePlan");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "BulkDealProperty");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "BulkDeal");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "BookingSchedulePaymentPlanDetail");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "BookingProcessingCharges");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "BookingJointMember");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "Banner");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "ApprovalUsers");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "ApprovalUI");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "ApprovalSetup");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "AdvanceApplication");

            migrationBuilder.AddColumn<string>(
                name: "LDAPlotNo",
                table: "StockCreations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LDAPlotNo",
                table: "StockCreations");

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "WithHoldingTaxPropertyWise",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "WithHoldingTax",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "WeekSchedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "WeekScheduleExective",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "ViolationGroupType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "ViolationGroup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "UserPermissionMapping",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "TransferReceiptProcessing",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "TransferHistoryNominee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "TransferHistoryJointMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "TransferHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "TestApproval",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "TanantDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "SurrenderHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "Surrender",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "StoreRoomFileMoving",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "StockCreationSetup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "SoftLock",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "SAPOperations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "SAPBillPostingCheck",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "SAPBilling",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "SaleTax",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "RolesPermissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "RePurchase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "RegNoProfileAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "RegistrationNoProfile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "ReadingOfficer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "ReadingDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "PropertyFixedChargesSetup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "Promotion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "PreSale",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "PlanInformation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "Permissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "PermissionForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "PaymentPlanSetup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "PaymentPlan",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "NewDemarcationRequestDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "NewDemarcationRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "NDCRequestForMemberCharges",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "NDCRequestForMemberAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "NDCRequestForMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "NDCRequestForDealerCharges",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "NDCRequestForDealerAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "NDCRequestForDealer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "NDC1PowerOfAttorey",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "NDC1CheckLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "NDC1Attachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "NDC1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "MeterType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "MeterStatus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "MeterReading",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "MeterInstallation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "MeterDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "MeterBillGenerationDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "MeterBillGeneration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "MemberSocialStatus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "MemberRelationshipHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "MemberProfile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "MemberNominees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "MemberInterest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "MemberAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "MapApprovalHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "LeadGenration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "LawyerData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "IndividualBillDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "IndividualBill",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "GracePeriodSetup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "GlobalChargeSetup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "GlobalChargeGroup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "GlobalChargeDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "GLDetermination",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "Forum",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "FormsChargeGroup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "FormAlertUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "FormAlerts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "FixedChargesEnableSetupPropertyWise",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "FixedChargeBillWHApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "FixedChargeBillDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "FixedChargeBill",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "FileVerificationRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "FileVerificationNDC1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "Demarcation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "DemandNoteItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "DemandNote",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "DealProperty",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "DealPaymentPlan",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "DealerWitness",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "Dealers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "DealerRelationshipHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "dealerEstateDeatails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "DealerCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "DealerAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "DealAdvanceApplicationRecipt",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "DealAdvanceApplicationHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "Deal",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "ConstructionSecurityLabour",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "ConstructionSecurityAttachment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "ConstructionSecurity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "ConstructionMonitoringStageDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "ConstructionMonitoring",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "ClientFileVerification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "ChargeGroupType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "CaseType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "CaseProfileParties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "CaseProfileNotices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "CaseProfileCaseHearings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "CaseProfileAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "CaseProfileAppeals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "CaseProfile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "CaseCategory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "BulkPaymentSchedule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "BulkDealProposePlan",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "BulkDealProperty",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "BulkDeal",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "BookingSchedulePaymentPlanDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "BookingProcessingCharges",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "BookingNominee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "BookingJointMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "Banner",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "ApprovalUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "ApprovalUI",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "ApprovalSetup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "Alerts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "AdvanceApplication",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
