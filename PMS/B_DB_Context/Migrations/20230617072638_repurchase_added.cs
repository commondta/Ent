using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class repurchase_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<string>(
            //    name: "Status",
            //    table: "SurrenderHistory",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)");

            //migrationBuilder.AlterColumn<string>(
            //    name: "Remarks",
            //    table: "SurrenderHistory",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)");

            //migrationBuilder.AddColumn<string>(
            //    name: "DealerName",
            //    table: "SurrenderHistory",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "EstateName",
            //    table: "SurrenderHistory",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Status",
            //    table: "Surrender",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)");

            //migrationBuilder.AlterColumn<string>(
            //    name: "Remarks",
            //    table: "Surrender",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)");

            //migrationBuilder.AddColumn<string>(
            //    name: "DealerName",
            //    table: "Surrender",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "EstateName",
            //    table: "Surrender",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "RePurchase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dealer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseRefundValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetProfitLoss = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalRecieved = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeductionAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Balance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_RePurchase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RePurchase_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "RePurchaseFinanceDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SapAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountDue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountRecieved = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RePurchaseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RePurchaseFinanceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RePurchaseFinanceDetails_RePurchase_RePurchaseId",
                        column: x => x.RePurchaseId,
                        principalTable: "RePurchase",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RePurchasePropertyDivisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegNumber = table.Column<int>(type: "int", nullable: true),
                    RegPostfix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropNumber = table.Column<int>(type: "int", nullable: true),
                    PropPostfix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RePurchaseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RePurchasePropertyDivisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RePurchasePropertyDivisions_RePurchase_RePurchaseId",
                        column: x => x.RePurchaseId,
                        principalTable: "RePurchase",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RePurchase_StockCreationId",
                table: "RePurchase",
                column: "StockCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_RePurchaseFinanceDetails_RePurchaseId",
                table: "RePurchaseFinanceDetails",
                column: "RePurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_RePurchasePropertyDivisions_RePurchaseId",
                table: "RePurchasePropertyDivisions",
                column: "RePurchaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RePurchaseFinanceDetails");

            migrationBuilder.DropTable(
                name: "RePurchasePropertyDivisions");

            migrationBuilder.DropTable(
                name: "RePurchase");

            //    migrationBuilder.DropColumn(
            //        name: "DealerName",
            //        table: "SurrenderHistory");

            //    migrationBuilder.DropColumn(
            //        name: "EstateName",
            //        table: "SurrenderHistory");

            //    migrationBuilder.DropColumn(
            //        name: "DealerName",
            //        table: "Surrender");

            //    migrationBuilder.DropColumn(
            //        name: "EstateName",
            //        table: "Surrender");

            //    migrationBuilder.AlterColumn<string>(
            //        name: "Status",
            //        table: "SurrenderHistory",
            //        type: "nvarchar(max)",
            //        nullable: false,
            //        defaultValue: "",
            //        oldClrType: typeof(string),
            //        oldType: "nvarchar(max)",
            //        oldNullable: true);

            //    migrationBuilder.AlterColumn<string>(
            //        name: "Remarks",
            //        table: "SurrenderHistory",
            //        type: "nvarchar(max)",
            //        nullable: false,
            //        defaultValue: "",
            //        oldClrType: typeof(string),
            //        oldType: "nvarchar(max)",
            //        oldNullable: true);

            //    migrationBuilder.AlterColumn<string>(
            //        name: "Status",
            //        table: "Surrender",
            //        type: "nvarchar(max)",
            //        nullable: false,
            //        defaultValue: "",
            //        oldClrType: typeof(string),
            //        oldType: "nvarchar(max)",
            //        oldNullable: true);

            //    migrationBuilder.AlterColumn<string>(
            //        name: "Remarks",
            //        table: "Surrender",
            //        type: "nvarchar(max)",
            //        nullable: false,
            //        defaultValue: "",
            //        oldClrType: typeof(string),
            //        oldType: "nvarchar(max)",
            //        oldNullable: true);
            //}
        }
    }
}
