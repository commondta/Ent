using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class removestanderedsizestockcreation_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StandardSize",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "StandardSizeUnit",
                table: "StockCreations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StandardSize",
                table: "StockCreations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StandardSizeUnit",
                table: "StockCreations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
