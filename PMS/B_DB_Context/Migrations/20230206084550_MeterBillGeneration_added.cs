using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class MeterBillGeneration_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeterBillGeneration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillFor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChargesStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsChangedFromIndivualBill = table.Column<bool>(type: "bit", nullable: true),
                    IsMeterBillGenerationRequested = table.Column<bool>(type: "bit", nullable: true),
                    IsMeterBillGenerationApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterBillGeneration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeterBillGenerationDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeterNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalUnitConsumed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PerUnitRate = table.Column<int>(type: "int", nullable: false),
                    FuelAdjustment = table.Column<int>(type: "int", nullable: false),
                    GrossAmount = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    NetAmount = table.Column<int>(type: "int", nullable: false),
                    MeterBillGenerationId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterBillGenerationDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeterBillGenerationDetail_MeterBillGeneration_MeterBillGenerationId",
                        column: x => x.MeterBillGenerationId,
                        principalTable: "MeterBillGeneration",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeterBillGenerationDetail_MeterBillGenerationId",
                table: "MeterBillGenerationDetail",
                column: "MeterBillGenerationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeterBillGenerationDetail");

            migrationBuilder.DropTable(
                name: "MeterBillGeneration");
        }
    }
}
