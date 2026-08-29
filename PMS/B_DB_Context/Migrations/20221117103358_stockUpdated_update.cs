using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class stockUpdated_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is_ClearnceApproved",
                table: "StockCreations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Is_ClearnceRequested",
                table: "StockCreations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Is_DemarcationApproved",
                table: "StockCreations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Is_DemarcationRequested",
                table: "StockCreations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Is_MapApprovalApproved",
                table: "StockCreations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Is_MapApprovalRequested",
                table: "StockCreations",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_ClearnceApproved",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "Is_ClearnceRequested",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "Is_DemarcationApproved",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "Is_DemarcationRequested",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "Is_MapApprovalApproved",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "Is_MapApprovalRequested",
                table: "StockCreations");
        }
    }
}
