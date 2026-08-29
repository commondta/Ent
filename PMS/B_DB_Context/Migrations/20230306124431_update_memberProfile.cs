using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class update_memberProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SocialStatus",
                table: "MemberSocialStatus",
                newName: "Description");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "MemberSocialStatus",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MatchId",
                table: "MemberSocialStatus",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "MemberSocialStatus");

            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "MemberSocialStatus");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "MemberSocialStatus",
                newName: "SocialStatus");
        }
    }
}
