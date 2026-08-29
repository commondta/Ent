using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class somefieldsremoved_paymentplansetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllDuesClear",
                table: "PaymentPlanSetup");

            migrationBuilder.DropColumn(
                name: "DevelopmentCharges",
                table: "PaymentPlanSetup");

            migrationBuilder.DropColumn(
                name: "PartialDevelopmentCharges",
                table: "PaymentPlanSetup");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllDuesClear",
                table: "PaymentPlanSetup",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "DevelopmentCharges",
                table: "PaymentPlanSetup",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PartialDevelopmentCharges",
                table: "PaymentPlanSetup",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
