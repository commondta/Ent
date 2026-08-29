using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class layout_changes_renumberandcop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentPropertyMemberAddress",
                table: "RenumberHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentPropertyMemberMobile",
                table: "RenumberHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentPropertyMemberAddress",
                table: "COPHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentPropertyMemberCnic",
                table: "COPHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentPropertyMemberMobile",
                table: "COPHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProposedPropertyMemberAddress",
                table: "COPHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProposedPropertyMemberCnic",
                table: "COPHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProposedPropertyMemberMobile",
                table: "COPHistories",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPropertyMemberAddress",
                table: "RenumberHistories");

            migrationBuilder.DropColumn(
                name: "CurrentPropertyMemberMobile",
                table: "RenumberHistories");

            migrationBuilder.DropColumn(
                name: "CurrentPropertyMemberAddress",
                table: "COPHistories");

            migrationBuilder.DropColumn(
                name: "CurrentPropertyMemberCnic",
                table: "COPHistories");

            migrationBuilder.DropColumn(
                name: "CurrentPropertyMemberMobile",
                table: "COPHistories");

            migrationBuilder.DropColumn(
                name: "ProposedPropertyMemberAddress",
                table: "COPHistories");

            migrationBuilder.DropColumn(
                name: "ProposedPropertyMemberCnic",
                table: "COPHistories");

            migrationBuilder.DropColumn(
                name: "ProposedPropertyMemberMobile",
                table: "COPHistories");
        }
    }
}
