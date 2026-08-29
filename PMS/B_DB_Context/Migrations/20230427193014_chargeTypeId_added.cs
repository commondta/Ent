using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class chargeTypeId_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingAccount",
                table: "SAPOperations");

            migrationBuilder.DropColumn(
                name: "CustomerSeries",
                table: "SAPOperations");

            migrationBuilder.DropColumn(
                name: "DealerAccountCode",
                table: "SAPOperations");

            migrationBuilder.DropColumn(
                name: "MemberAccountCode",
                table: "SAPOperations");

            migrationBuilder.DropColumn(
                name: "BillFor",
                table: "SAPBillPostingCheck");

            migrationBuilder.DropColumn(
                name: "BillMonth",
                table: "SAPBillPostingCheck");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "SAPBillPostingCheck");

            migrationBuilder.DropColumn(
                name: "DocDate",
                table: "SAPBillPostingCheck");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "SAPBillPostingCheck");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookingAccount",
                table: "SAPOperations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerSeries",
                table: "SAPOperations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DealerAccountCode",
                table: "SAPOperations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MemberAccountCode",
                table: "SAPOperations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillFor",
                table: "SAPBillPostingCheck",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillMonth",
                table: "SAPBillPostingCheck",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "SAPBillPostingCheck",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DocDate",
                table: "SAPBillPostingCheck",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "SAPBillPostingCheck",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
