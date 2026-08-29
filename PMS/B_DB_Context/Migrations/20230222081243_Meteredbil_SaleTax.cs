using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class Meteredbil_SaleTax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "MeterBillGenerationDetail",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SaleTax",
                table: "MeterBillGenerationDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SaleTaxAmount",
                table: "MeterBillGenerationDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SaleTax",
                table: "FixedChargeBillDetail",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "MeterBillGenerationDetail");

            migrationBuilder.DropColumn(
                name: "SaleTax",
                table: "MeterBillGenerationDetail");

            migrationBuilder.DropColumn(
                name: "SaleTaxAmount",
                table: "MeterBillGenerationDetail");

            migrationBuilder.AlterColumn<string>(
                name: "SaleTax",
                table: "FixedChargeBillDetail",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
