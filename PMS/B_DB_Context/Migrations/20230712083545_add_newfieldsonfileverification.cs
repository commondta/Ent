using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class add_newfieldsonfileverification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceivedBy",
                table: "FileVerificationRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecieverCNIC",
                table: "FileVerificationRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecieverFatherName",
                table: "FileVerificationRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecieverMobile",
                table: "FileVerificationRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FileVerificationAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoucmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileVerificationRequestId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileVerificationAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileVerificationAttachments_FileVerificationRequests_FileVerificationRequestId",
                        column: x => x.FileVerificationRequestId,
                        principalTable: "FileVerificationRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileVerificationAttachments_FileVerificationRequestId",
                table: "FileVerificationAttachments",
                column: "FileVerificationRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileVerificationAttachments");

            migrationBuilder.DropColumn(
                name: "ReceivedBy",
                table: "FileVerificationRequests");

            migrationBuilder.DropColumn(
                name: "RecieverCNIC",
                table: "FileVerificationRequests");

            migrationBuilder.DropColumn(
                name: "RecieverFatherName",
                table: "FileVerificationRequests");

            migrationBuilder.DropColumn(
                name: "RecieverMobile",
                table: "FileVerificationRequests");
        }
    }
}
