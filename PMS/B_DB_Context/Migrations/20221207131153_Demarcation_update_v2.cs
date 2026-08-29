using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class Demarcation_update_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is_DemarcationFormApproved",
                table: "StockCreations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Is_DemarcationFormRequested",
                table: "StockCreations",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_DemarcationFormApproved",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "Is_DemarcationFormRequested",
                table: "StockCreations");
        }
    }
}
