using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class addfields_in_NDCForMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SapAccount",
                table: "NDCRequestForMemberCharges",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "NDCRequestForMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DealerCode",
                table: "NDCRequestForMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DealerName",
                table: "NDCRequestForMember",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SapAccount",
                table: "NDCRequestForMemberCharges");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "DealerCode",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "DealerName",
                table: "NDCRequestForMember");
        }
    }
}
