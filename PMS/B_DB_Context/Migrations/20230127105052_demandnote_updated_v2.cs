using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class demandnote_updated_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDemandNoteApproved",
                table: "DemandNote",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDemandNoteRequested",
                table: "DemandNote",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDemandNoteApproved",
                table: "DemandNote");

            migrationBuilder.DropColumn(
                name: "IsDemandNoteRequested",
                table: "DemandNote");
        }
    }
}
