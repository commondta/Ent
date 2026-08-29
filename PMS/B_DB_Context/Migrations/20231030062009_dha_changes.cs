using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class dha_changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "TransferReceiptProcessing",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DealerCode",
                table: "TransferReceiptProcessing",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DealerName",
                table: "TransferReceiptProcessing",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstateName",
                table: "TransferReceiptProcessing",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NDCRequestType",
                table: "TransferReceiptProcessing",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SlotDate",
                table: "TransferReceiptProcessing",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SlotHour",
                table: "TransferReceiptProcessing",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SlotMintues",
                table: "TransferReceiptProcessing",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransferType",
                table: "TransferReceiptProcessing",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DealerCode",
                table: "TransferHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DealerName",
                table: "TransferHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstateName",
                table: "TransferHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstateName",
                table: "NDCRequestForMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "NDCRequestForMember",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignVerification",
                table: "NDCRequestForMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstateName",
                table: "NDC1",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "DealerCode",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "DealerName",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "EstateName",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "NDCRequestType",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "SlotDate",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "SlotHour",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "SlotMintues",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "TransferType",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "DealerCode",
                table: "TransferHistory");

            migrationBuilder.DropColumn(
                name: "DealerName",
                table: "TransferHistory");

            migrationBuilder.DropColumn(
                name: "EstateName",
                table: "TransferHistory");

            migrationBuilder.DropColumn(
                name: "EstateName",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "SignVerification",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "EstateName",
                table: "NDC1");
        }
    }
}
