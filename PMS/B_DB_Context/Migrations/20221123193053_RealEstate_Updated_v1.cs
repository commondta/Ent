using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class RealEstate_Updated_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalSetup_ApprovalUI_ApprovalUiId",
                table: "ApprovalSetup");

            migrationBuilder.RenameColumn(
                name: "ApprovalUiId",
                table: "ApprovalSetup",
                newName: "ApprovalUIId");

            migrationBuilder.RenameIndex(
                name: "IX_ApprovalSetup_ApprovalUiId",
                table: "ApprovalSetup",
                newName: "IX_ApprovalSetup_ApprovalUIId");

            migrationBuilder.AddColumn<int>(
                name: "PhaseId",
                table: "Real_Estates",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalSetup_ApprovalUI_ApprovalUIId",
                table: "ApprovalSetup",
                column: "ApprovalUIId",
                principalTable: "ApprovalUI",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalSetup_ApprovalUI_ApprovalUIId",
                table: "ApprovalSetup");

            migrationBuilder.DropColumn(
                name: "PhaseId",
                table: "Real_Estates");

            migrationBuilder.RenameColumn(
                name: "ApprovalUIId",
                table: "ApprovalSetup",
                newName: "ApprovalUiId");

            migrationBuilder.RenameIndex(
                name: "IX_ApprovalSetup_ApprovalUIId",
                table: "ApprovalSetup",
                newName: "IX_ApprovalSetup_ApprovalUiId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalSetup_ApprovalUI_ApprovalUiId",
                table: "ApprovalSetup",
                column: "ApprovalUiId",
                principalTable: "ApprovalUI",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
