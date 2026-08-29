using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class adding_colummns_DemandNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustodianApprovedOrRejectRemarks",
                table: "DemandNote",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CustodianApproved_At",
                table: "DemandNote",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CustodianAssigned",
                table: "DemandNote",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustodianId",
                table: "DemandNote",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CustodianRejected_At",
                table: "DemandNote",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerApprovedOrRejectRemarks",
                table: "DemandNote",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ManagerApproved_At",
                table: "DemandNote",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ManagerAssigned",
                table: "DemandNote",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "DemandNote",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ManagerRejected_At",
                table: "DemandNote",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustodianApprovedOrRejectRemarks",
                table: "DemandNote");

            migrationBuilder.DropColumn(
                name: "CustodianApproved_At",
                table: "DemandNote");

            migrationBuilder.DropColumn(
                name: "CustodianAssigned",
                table: "DemandNote");

            migrationBuilder.DropColumn(
                name: "CustodianId",
                table: "DemandNote");

            migrationBuilder.DropColumn(
                name: "CustodianRejected_At",
                table: "DemandNote");

            migrationBuilder.DropColumn(
                name: "ManagerApprovedOrRejectRemarks",
                table: "DemandNote");

            migrationBuilder.DropColumn(
                name: "ManagerApproved_At",
                table: "DemandNote");

            migrationBuilder.DropColumn(
                name: "ManagerAssigned",
                table: "DemandNote");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "DemandNote");

            migrationBuilder.DropColumn(
                name: "ManagerRejected_At",
                table: "DemandNote");
        }
    }
}
