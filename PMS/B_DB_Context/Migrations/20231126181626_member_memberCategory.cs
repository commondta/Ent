using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class member_memberCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Force",
                table: "MemberProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MemberCategory",
                table: "MemberProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PANO",
                table: "MemberProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rank",
                table: "MemberProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Shaheed",
                table: "MemberProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Force",
                table: "MemberProfile");

            migrationBuilder.DropColumn(
                name: "MemberCategory",
                table: "MemberProfile");

            migrationBuilder.DropColumn(
                name: "PANO",
                table: "MemberProfile");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "MemberProfile");

            migrationBuilder.DropColumn(
                name: "Shaheed",
                table: "MemberProfile");
        }
    }
}
