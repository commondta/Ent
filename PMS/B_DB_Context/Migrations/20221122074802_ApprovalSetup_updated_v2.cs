using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class ApprovalSetup_updated_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalSetup_ApprovalUI_ApprovalUIId",
                table: "ApprovalSetup");

            migrationBuilder.DropColumn(
                name: "ApprovalDocumentId",
                table: "ApprovalSetup");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovalUIId",
                table: "ApprovalSetup",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<int>(
                name: "ApprovalUIId",
                table: "ApprovalSetup",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ApprovalDocumentId",
                table: "ApprovalSetup",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalSetup_ApprovalUI_ApprovalUIId",
                table: "ApprovalSetup",
                column: "ApprovalUIId",
                principalTable: "ApprovalUI",
                principalColumn: "Id");
        }
    }
}
