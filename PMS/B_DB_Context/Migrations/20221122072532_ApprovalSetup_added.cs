using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class ApprovalSetup_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApprovalSetup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovalDocumentId = table.Column<int>(type: "int", nullable: false),
                    ApprovalUIId = table.Column<int>(type: "int", nullable: true),
                    StageNo = table.Column<int>(type: "int", nullable: false),
                    NumberOfApprovalRequired = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalSetup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalSetup_ApprovalUI_ApprovalUIId",
                        column: x => x.ApprovalUIId,
                        principalTable: "ApprovalUI",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApprovalUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovalSetupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserDesignation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalUsers_ApprovalSetup_ApprovalSetupId",
                        column: x => x.ApprovalSetupId,
                        principalTable: "ApprovalSetup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalSetup_ApprovalUIId",
                table: "ApprovalSetup",
                column: "ApprovalUIId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalUsers_ApprovalSetupId",
                table: "ApprovalUsers",
                column: "ApprovalSetupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalUsers");

            migrationBuilder.DropTable(
                name: "ApprovalSetup");
        }
    }
}
