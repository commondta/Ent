using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class PreSaleadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreSale",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cnic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ByCareOf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealerCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaleBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranscationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OneTimePayment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Installments = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_PreSale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreSale_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PaymentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rebate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreSaleId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentDetails_PreSale_PreSaleId",
                        column: x => x.PreSaleId,
                        principalTable: "PreSale",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PaymentPlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Days = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentFor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreSaleId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentPlan_PreSale_PreSaleId",
                        column: x => x.PreSaleId,
                        principalTable: "PreSale",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TermsConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TranscationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreSaleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermsConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TermsConditions_PreSale_PreSaleId",
                        column: x => x.PreSaleId,
                        principalTable: "PreSale",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_PreSaleId",
                table: "PaymentDetails",
                column: "PreSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentPlan_PreSaleId",
                table: "PaymentPlan",
                column: "PreSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_PreSale_StockCreationId",
                table: "PreSale",
                column: "StockCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_TermsConditions_PreSaleId",
                table: "TermsConditions",
                column: "PreSaleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentDetails");

            migrationBuilder.DropTable(
                name: "PaymentPlan");

            migrationBuilder.DropTable(
                name: "TermsConditions");

            migrationBuilder.DropTable(
                name: "PreSale");
        }
    }
}
