using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class wTaxAmount_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WTaxAmount",
                table: "MeterBillGenerationDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WTaxAmount",
                table: "FixedChargeBillDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WTaxAmount",
                table: "MeterBillGenerationDetail");

            migrationBuilder.DropColumn(
                name: "WTaxAmount",
                table: "FixedChargeBillDetail");
        }
    }
}
