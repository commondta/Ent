using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class memberprofile_stockCreation_updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InventoryStatus",
                table: "StockCreations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CNICBack",
                table: "MemberProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CNICFront",
                table: "MemberProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InventoryStatus",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "CNICBack",
                table: "MemberProfile");

            migrationBuilder.DropColumn(
                name: "CNICFront",
                table: "MemberProfile");
        }
    }
}
