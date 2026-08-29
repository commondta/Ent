using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class ApprovalUI_Updated_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is_ConstructionMonitoringApproved",
                table: "StockCreations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Is_ConstructionMonitoringRequested",
                table: "StockCreations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SerialNo",
                table: "ApprovalUI",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_ConstructionMonitoringApproved",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "Is_ConstructionMonitoringRequested",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "SerialNo",
                table: "ApprovalUI");
        }
    }
}
