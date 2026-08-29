using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class addnewfieldforSAP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocEntry",
                table: "MemberProfile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocNum",
                table: "MemberProfile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SapPosting",
                table: "MemberProfile",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocEntry",
                table: "MemberProfile");

            migrationBuilder.DropColumn(
                name: "DocNum",
                table: "MemberProfile");

            migrationBuilder.DropColumn(
                name: "SapPosting",
                table: "MemberProfile");
        }
    }
}
