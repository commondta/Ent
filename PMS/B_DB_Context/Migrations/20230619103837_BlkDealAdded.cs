using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class BlkDealAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.CreateTable(
                name: "BulkDeal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerId = table.Column<int>(type: "int", nullable: true),
                    DealName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealNature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QtyProperty = table.Column<int>(type: "int", nullable: true),
                    DealDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DealExpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CommissionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Commission = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RebateType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rebate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NetReceivable = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalReceied = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OutstandingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GracePeriod = table.Column<int>(type: "int", nullable: true),
                    SurchargePerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OneTimePayment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Installment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_BulkDeal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BulkDeal_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BulkDealProperty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockId = table.Column<int>(type: "int", nullable: true),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RealStateType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Project = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Block = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rebate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NetReceivable = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReceiedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OutstandingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BulkDealId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BulkDealProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BulkDealProperty_BulkDeal_BulkDealId",
                        column: x => x.BulkDealId,
                        principalTable: "BulkDeal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BulkDealProposePlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BulkDealId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BulkDealProposePlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BulkDealProposePlan_BulkDeal_BulkDealId",
                        column: x => x.BulkDealId,
                        principalTable: "BulkDeal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BulkPaymentSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BulkDealId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BulkPaymentSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BulkPaymentSchedule_BulkDeal_BulkDealId",
                        column: x => x.BulkDealId,
                        principalTable: "BulkDeal",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BulkDeal_DealerId",
                table: "BulkDeal",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_BulkDealProperty_BulkDealId",
                table: "BulkDealProperty",
                column: "BulkDealId");

            migrationBuilder.CreateIndex(
                name: "IX_BulkDealProposePlan_BulkDealId",
                table: "BulkDealProposePlan",
                column: "BulkDealId");

            migrationBuilder.CreateIndex(
                name: "IX_BulkPaymentSchedule_BulkDealId",
                table: "BulkPaymentSchedule",
                column: "BulkDealId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BulkDealProperty");

            migrationBuilder.DropTable(
                name: "BulkDealProposePlan");

            migrationBuilder.DropTable(
                name: "BulkPaymentSchedule");

            migrationBuilder.DropTable(
                name: "BulkDeal");

        }
    }
}
