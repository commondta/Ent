using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class LeadGenrationupdate_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HonorificsContactPersContactPersoon",
                table: "LeadGenration",
                newName: "HonorificsContactPersoon");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HonorificsContactPersoon",
                table: "LeadGenration",
                newName: "HonorificsContactPersContactPersoon");
        }
    }
}
