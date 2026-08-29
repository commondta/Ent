using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class add_sapAccountincharge_Setup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxCode",
                table: "ChargeGroupType",
                newName: "SapAccount");

            migrationBuilder.AddColumn<string>(
                name: "ChargeType",
                table: "MeterBillGenerationDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "MeterBillGenerationDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SapAccount",
                table: "MeterBillGenerationDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SapAccount",
                table: "GlobalChargeDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SapAccount",
                table: "FixedChargeBillDetail",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChargeType",
                table: "MeterBillGenerationDetail");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "MeterBillGenerationDetail");

            migrationBuilder.DropColumn(
                name: "SapAccount",
                table: "MeterBillGenerationDetail");

            migrationBuilder.DropColumn(
                name: "SapAccount",
                table: "GlobalChargeDetail");

            migrationBuilder.DropColumn(
                name: "SapAccount",
                table: "FixedChargeBillDetail");

            migrationBuilder.RenameColumn(
                name: "SapAccount",
                table: "ChargeGroupType",
                newName: "TaxCode");
        }
    }
}
