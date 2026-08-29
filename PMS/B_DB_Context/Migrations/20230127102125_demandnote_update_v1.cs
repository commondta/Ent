using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class demandnote_update_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "InfoPrice",
                table: "DemandNoteItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LastPurcPrice",
                table: "DemandNoteItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Deparment",
                table: "DemandNote",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ItemGroupCode",
                table: "DemandNote",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InfoPrice",
                table: "DemandNoteItems");

            migrationBuilder.DropColumn(
                name: "LastPurcPrice",
                table: "DemandNoteItems");

            migrationBuilder.DropColumn(
                name: "Deparment",
                table: "DemandNote");

            migrationBuilder.DropColumn(
                name: "ItemGroupCode",
                table: "DemandNote");
        }
    }
}
