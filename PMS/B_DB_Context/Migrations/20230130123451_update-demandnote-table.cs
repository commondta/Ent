using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class updatedemandnotetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocEntry",
                table: "DemandNote",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DocNum",
                table: "DemandNote",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "SapPosting",
                table: "DemandNote",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocEntry",
                table: "DemandNote");

            migrationBuilder.DropColumn(
                name: "DocNum",
                table: "DemandNote");

            migrationBuilder.DropColumn(
                name: "SapPosting",
                table: "DemandNote");
        }
    }
}
