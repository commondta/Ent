using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class mapApproval_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Remarks",
                table: "MapApprovalHistory",
                newName: "ClientRemarks");

            migrationBuilder.AddColumn<string>(
                name: "ArchRemarks",
                table: "MapApprovalHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientStage",
                table: "MapApprovalHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Stage",
                table: "MapApprovalHistory",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArchRemarks",
                table: "MapApprovalHistory");

            migrationBuilder.DropColumn(
                name: "ClientStage",
                table: "MapApprovalHistory");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "MapApprovalHistory");

            migrationBuilder.RenameColumn(
                name: "ClientRemarks",
                table: "MapApprovalHistory",
                newName: "Remarks");
        }
    }
}
