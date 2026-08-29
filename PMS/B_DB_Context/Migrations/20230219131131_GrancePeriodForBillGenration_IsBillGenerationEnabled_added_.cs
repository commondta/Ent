using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class GrancePeriodForBillGenration_IsBillGenerationEnabled_added_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "GrancePeriodForBillGenration",
                table: "StockCreations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBillGenerationEnabled",
                table: "StockCreations",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GrancePeriodForBillGenration",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "IsBillGenerationEnabled",
                table: "StockCreations");
        }
    }
}
