using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class TaxStatus_add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "ConstructionSecurity");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClearanceOn",
                table: "StockCreations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberTaxStatus",
                table: "StockCreations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxStatus",
                table: "GlobalChargeSetup",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClearanceOn",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "MemberTaxStatus",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "TaxStatus",
                table: "GlobalChargeSetup");

            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "ConstructionSecurity",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
