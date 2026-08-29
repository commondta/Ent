using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class deals_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerId = table.Column<int>(type: "int", nullable: true),
                    DealNature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QtyProperty = table.Column<int>(type: "int", nullable: true),
                    DealDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DealExpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CommissionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Commission = table.Column<double>(type: "float", nullable: true),
                    RebateType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rebate = table.Column<double>(type: "float", nullable: true),
                    TotalValue = table.Column<double>(type: "float", nullable: true),
                    NetReceivable = table.Column<double>(type: "float", nullable: true),
                    TotalReceied = table.Column<double>(type: "float", nullable: true),
                    OutstandingBalance = table.Column<double>(type: "float", nullable: true),
                    GracePeriod = table.Column<int>(type: "int", nullable: true),
                    SurchargePerDay = table.Column<double>(type: "float", nullable: true),
                    OneTimePayment = table.Column<double>(type: "float", nullable: true),
                    Installment = table.Column<double>(type: "float", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDealRequested = table.Column<bool>(type: "bit", nullable: true),
                    IsDealApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DealProperty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockCreationId = table.Column<int>(type: "int", nullable: true),
                    DealId = table.Column<int>(type: "int", nullable: true),
                    Rebate = table.Column<double>(type: "float", nullable: true),
                    TotalAmount = table.Column<double>(type: "float", nullable: true),
                    NetReceivable = table.Column<double>(type: "float", nullable: true),
                    ReceiedAmount = table.Column<double>(type: "float", nullable: true),
                    OutstandingBalance = table.Column<double>(type: "float", nullable: true),
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
                    table.PrimaryKey("PK_DealProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealProperty_Deal_DealId",
                        column: x => x.DealId,
                        principalTable: "Deal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DealPaymentPlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealPropertyId = table.Column<int>(type: "int", nullable: true),
                    ChargeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrossAmount = table.Column<double>(type: "float", nullable: true),
                    Rebate = table.Column<double>(type: "float", nullable: true),
                    NetAmount = table.Column<double>(type: "float", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetTotal = table.Column<double>(type: "float", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealPaymentPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealPaymentPlan_DealProperty_DealPropertyId",
                        column: x => x.DealPropertyId,
                        principalTable: "DealProperty",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DealPaymentPlan_DealPropertyId",
                table: "DealPaymentPlan",
                column: "DealPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_DealProperty_DealId",
                table: "DealProperty",
                column: "DealId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DealPaymentPlan");

            migrationBuilder.DropTable(
                name: "DealProperty");

            migrationBuilder.DropTable(
                name: "Deal");
        }
    }
}
