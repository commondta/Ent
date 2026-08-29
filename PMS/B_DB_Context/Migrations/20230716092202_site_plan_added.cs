using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class site_plan_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LDAPlotNo",
                table: "Demarcation");

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "MeterPhaseWiseRates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "MeterPhase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "FileVerificationAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "FileReceivingRegisters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedUserName",
                table: "FileDocDupRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SitePlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsSitePlanRequested = table.Column<bool>(type: "bit", nullable: true),
                    IsSitePlanApproved = table.Column<bool>(type: "bit", nullable: true),
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
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    LastModifiedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SitePlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SitePlans_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SitePlans_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SitePlanAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoucmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SitePlanId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    LastModifiedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SitePlanAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SitePlanAttachments_SitePlans_SitePlanId",
                        column: x => x.SitePlanId,
                        principalTable: "SitePlans",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SitePlanAttachments_SitePlanId",
                table: "SitePlanAttachments",
                column: "SitePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_SitePlans_MemberProfileId",
                table: "SitePlans",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SitePlans_StockCreationId",
                table: "SitePlans",
                column: "StockCreationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SitePlanAttachments");

            migrationBuilder.DropTable(
                name: "SitePlans");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "MeterPhaseWiseRates");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "MeterPhase");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "FileVerificationAttachments");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "FileReceivingRegisters");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserName",
                table: "FileDocDupRequests");

            migrationBuilder.AddColumn<string>(
                name: "LDAPlotNo",
                table: "Demarcation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
