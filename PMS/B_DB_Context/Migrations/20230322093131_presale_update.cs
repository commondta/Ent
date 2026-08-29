using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class presale_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "PaymentPlanTotal",
                table: "PreSale");

            migrationBuilder.AlterColumn<string>(
                name: "TranscationType",
                table: "TermsConditions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "OneTimePayment",
                table: "PreSale",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Installments",
                table: "PreSale",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NetCost",
                table: "PreSale",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlanCode",
                table: "PreSale",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalCost",
                table: "PreSale",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalRebate",
                table: "PreSale",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentType",
                table: "PaymentPlan",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentFor",
                table: "PaymentPlan",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Days",
                table: "PaymentPlan",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "NetAmount",
                table: "PaymentPlan",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "PaymentPlan",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Rebate",
                table: "PaymentPlan",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NetCost",
                table: "PreSale");

            migrationBuilder.DropColumn(
                name: "PlanCode",
                table: "PreSale");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "PreSale");

            migrationBuilder.DropColumn(
                name: "TotalRebate",
                table: "PreSale");

            migrationBuilder.DropColumn(
                name: "NetAmount",
                table: "PaymentPlan");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "PaymentPlan");

            migrationBuilder.DropColumn(
                name: "Rebate",
                table: "PaymentPlan");

            migrationBuilder.AlterColumn<string>(
                name: "TranscationType",
                table: "TermsConditions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "OneTimePayment",
                table: "PreSale",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Installments",
                table: "PreSale",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentPlanTotal",
                table: "PreSale",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentType",
                table: "PaymentPlan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentFor",
                table: "PaymentPlan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Days",
                table: "PaymentPlan",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreSaleId = table.Column<int>(type: "int", nullable: true),
                    ChargeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    NetTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rebate = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_PreSaleId",
                table: "PaymentDetails",
                column: "PreSaleId");
        }
    }
}
