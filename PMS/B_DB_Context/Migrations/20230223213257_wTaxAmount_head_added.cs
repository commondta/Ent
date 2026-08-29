using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class wTaxAmount_head_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WTaxAmount",
                table: "FixedChargeBillDetail");

            migrationBuilder.AddColumn<int>(
                name: "WTaxAmount",
                table: "FixedChargeBill",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WTaxAmount",
                table: "FixedChargeBill");

            migrationBuilder.AddColumn<int>(
                name: "WTaxAmount",
                table: "FixedChargeBillDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
