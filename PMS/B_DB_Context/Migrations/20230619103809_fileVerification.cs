using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class fileVerification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileVerificationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsFileVerificationRequested = table.Column<bool>(type: "bit", nullable: true),
                    IsFileVerificationApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsRequestClosed = table.Column<bool>(type: "bit", nullable: true),
                    Block = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PossessionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConstrucationStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberProfileId = table.Column<int>(type: "int", nullable: true),
                    StockCreationId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileVerificationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileVerificationRequests_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FileVerificationRequests_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "FileVerificationRequestCharges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SapAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileVerificationRequestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileVerificationRequestCharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileVerificationRequestCharges_FileVerificationRequests_FileVerificationRequestId",
                        column: x => x.FileVerificationRequestId,
                        principalTable: "FileVerificationRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileVerificationRequestCharges_FileVerificationRequestId",
                table: "FileVerificationRequestCharges",
                column: "FileVerificationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_FileVerificationRequests_MemberProfileId",
                table: "FileVerificationRequests",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_FileVerificationRequests_StockCreationId",
                table: "FileVerificationRequests",
                column: "StockCreationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileVerificationRequestCharges");

            migrationBuilder.DropTable(
                name: "FileVerificationRequests");
        }
    }
}
