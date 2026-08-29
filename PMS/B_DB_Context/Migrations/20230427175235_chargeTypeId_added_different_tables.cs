using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class chargeTypeId_added_different_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChargeTypeId",
                table: "PlanInformation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChargeTypeId",
                table: "PaymentPlan",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChargeTypeId",
                table: "BookingSchedulePaymentPlanDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChargeTypeId",
                table: "BookingProcessingCharges",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChargeTypeId",
                table: "PlanInformation");

            migrationBuilder.DropColumn(
                name: "ChargeTypeId",
                table: "PaymentPlan");

            migrationBuilder.DropColumn(
                name: "ChargeTypeId",
                table: "BookingSchedulePaymentPlanDetail");

            migrationBuilder.DropColumn(
                name: "ChargeTypeId",
                table: "BookingProcessingCharges");
        }
    }
}
