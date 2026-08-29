using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class fileRequest_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileRequestType",
                table: "GlobalChargeSetup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FileDocDupRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsFileDocDupRequested = table.Column<bool>(type: "bit", nullable: true),
                    IsFileDocDupApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsRequestClosed = table.Column<bool>(type: "bit", nullable: true),
                    RequestType = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_FileDocDupRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileDocDupRequests_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FileDocDupRequests_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "FileDocDupRequestedCharges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SapAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileVerificationRequestId = table.Column<int>(type: "int", nullable: true),
                    FileDocDupRequestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDocDupRequestedCharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileDocDupRequestedCharges_FileDocDupRequests_FileDocDupRequestId",
                        column: x => x.FileDocDupRequestId,
                        principalTable: "FileDocDupRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FileDocDupRequestedCharges_FileVerificationRequests_FileVerificationRequestId",
                        column: x => x.FileVerificationRequestId,
                        principalTable: "FileVerificationRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileDocDupRequestedCharges_FileDocDupRequestId",
                table: "FileDocDupRequestedCharges",
                column: "FileDocDupRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_FileDocDupRequestedCharges_FileVerificationRequestId",
                table: "FileDocDupRequestedCharges",
                column: "FileVerificationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_FileDocDupRequests_MemberProfileId",
                table: "FileDocDupRequests",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_FileDocDupRequests_StockCreationId",
                table: "FileDocDupRequests",
                column: "StockCreationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileDocDupRequestedCharges");

            migrationBuilder.DropTable(
                name: "FileDocDupRequests");

            migrationBuilder.DropColumn(
                name: "FileRequestType",
                table: "GlobalChargeSetup");
        }
    }
}
