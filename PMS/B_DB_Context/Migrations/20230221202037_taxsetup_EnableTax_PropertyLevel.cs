using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class taxsetup_EnableTax_PropertyLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSaleTaxEnabled",
                table: "StockCreations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsWithHoldingTaxEnabled",
                table: "StockCreations",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSaleTaxEnabled",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "IsWithHoldingTaxEnabled",
                table: "StockCreations");
        }
    }
}
