using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class fixedchargebill_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FixedChargeBill",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillFor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFixedChargeBillRequested = table.Column<bool>(type: "bit", nullable: true),
                    IsFixedChargeBillApproved = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_FixedChargeBill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedChargeBill_StockCreations_StockCreationID",
                        column: x => x.StockCreationID,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "FixedChargeBillDetail",
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
                    FixedChargeBillId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixedChargeBillDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedChargeBillDetail_FixedChargeBill_FixedChargeBillId",
                        column: x => x.FixedChargeBillId,
                        principalTable: "FixedChargeBill",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FixedChargeBill_StockCreationID",
                table: "FixedChargeBill",
                column: "StockCreationID");

            migrationBuilder.CreateIndex(
                name: "IX_FixedChargeBillDetail_FixedChargeBillId",
                table: "FixedChargeBillDetail",
                column: "FixedChargeBillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FixedChargeBillDetail");

            migrationBuilder.DropTable(
                name: "FixedChargeBill");
        }
    }
}
