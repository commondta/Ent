using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class globalChargeSetupId_add_InFixedchargeSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstallementDate",
                table: "BookingSchedulePaymentPlanDetail");

            migrationBuilder.AddColumn<int>(
                name: "GlobalChargeSetupId",
                table: "PropertyFixedChargesSetup",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TransferReceiptProcessing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlockName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RealStateTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlotSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConstructionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Filer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuyerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransferDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberProfileId = table.Column<int>(type: "int", nullable: false),
                    StockCreationId = table.Column<int>(type: "int", nullable: false),
                    CoveredArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SellerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SellerFilerStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferReceiptProcessing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferReceiptProcessing_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferReceiptProcessing_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferReceiptProcessing_MemberProfileId",
                table: "TransferReceiptProcessing",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferReceiptProcessing_StockCreationId",
                table: "TransferReceiptProcessing",
                column: "StockCreationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "GlobalChargeSetupId",
                table: "PropertyFixedChargesSetup");

            migrationBuilder.AddColumn<DateTime>(
                name: "InstallementDate",
                table: "BookingSchedulePaymentPlanDetail",
                type: "datetime2",
                nullable: true);
        }
    }
}
