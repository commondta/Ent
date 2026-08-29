using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class ClientFileVerification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientFileVerification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockCreationId = table.Column<int>(type: "int", nullable: true),
                    MemberProfileId = table.Column<int>(type: "int", nullable: true),
                    IsClientFileVerificationRequested = table.Column<bool>(type: "bit", nullable: true),
                    IsClientFileVerificationApproved = table.Column<bool>(type: "bit", nullable: true),
                    SendForApproval = table.Column<bool>(type: "bit", nullable: true),
                    ReceivedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceivedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecieverFatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecieverCNIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecieverMobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPrintEnabled = table.Column<bool>(type: "bit", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFileVerification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientFileVerification_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClientFileVerification_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ClientFileVerificationAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoucmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientFileVerificationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFileVerificationAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientFileVerificationAttachments_ClientFileVerification_ClientFileVerificationId",
                        column: x => x.ClientFileVerificationId,
                        principalTable: "ClientFileVerification",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientFileVerification_MemberProfileId",
                table: "ClientFileVerification",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientFileVerification_StockCreationId",
                table: "ClientFileVerification",
                column: "StockCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientFileVerificationAttachments_ClientFileVerificationId",
                table: "ClientFileVerificationAttachments",
                column: "ClientFileVerificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientFileVerificationAttachments");

            migrationBuilder.DropTable(
                name: "ClientFileVerification");
        }
    }
}
