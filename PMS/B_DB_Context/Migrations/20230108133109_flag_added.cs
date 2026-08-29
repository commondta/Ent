using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class flag_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMemberProfileApproved",
                table: "MemberProfile",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMemberProfileRequested",
                table: "MemberProfile",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDealerProfileApproved",
                table: "Dealers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDealerProfileRequested",
                table: "Dealers",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMemberProfileApproved",
                table: "MemberProfile");

            migrationBuilder.DropColumn(
                name: "IsMemberProfileRequested",
                table: "MemberProfile");

            migrationBuilder.DropColumn(
                name: "IsDealerProfileApproved",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "IsDealerProfileRequested",
                table: "Dealers");
        }
    }
}
