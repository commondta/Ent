using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class IndividualBillGeneration_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IndividualBill",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillFor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsIndividualBillRequested = table.Column<bool>(type: "bit", nullable: true),
                    IsIndividualBillApproved = table.Column<bool>(type: "bit", nullable: true),
                    TotalAmount = table.Column<int>(type: "int", nullable: false),
                    StockCreationID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualBill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndividualBill_StockCreations_StockCreationID",
                        column: x => x.StockCreationID,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "IndividualBillDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Surcharge = table.Column<int>(type: "int", nullable: false),
                    OtherDuesDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OtherDuesAmount = table.Column<int>(type: "int", nullable: false),
                    GrossAmount = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    NetAmount = table.Column<int>(type: "int", nullable: false),
                    IndividualBillId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualBillDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndividualBillDetail_IndividualBill_IndividualBillId",
                        column: x => x.IndividualBillId,
                        principalTable: "IndividualBill",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndividualBill_StockCreationID",
                table: "IndividualBill",
                column: "StockCreationID");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualBillDetail_IndividualBillId",
                table: "IndividualBillDetail",
                column: "IndividualBillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndividualBillDetail");

            migrationBuilder.DropTable(
                name: "IndividualBill");
        }
    }
}
