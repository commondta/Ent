using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class stock_creation_v11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is_StockCreationApproved",
                table: "StockCreations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Is_StockCreationRequested",
                table: "StockCreations",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_StockCreationApproved",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "Is_StockCreationRequested",
                table: "StockCreations");
        }
    }
}
