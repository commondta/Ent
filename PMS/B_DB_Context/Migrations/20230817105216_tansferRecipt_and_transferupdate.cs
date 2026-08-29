using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class tansferRecipt_and_transferupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TransferDate",
                table: "TransferReceiptProcessing",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConstructedYears",
                table: "TransferReceiptProcessing",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropertyTaxYears",
                table: "TransferReceiptProcessing",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReciptPrpcessingId",
                table: "TransferHistory",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConstructedYears",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "PropertyTaxYears",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "ReciptPrpcessingId",
                table: "TransferHistory");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TransferDate",
                table: "TransferReceiptProcessing",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
