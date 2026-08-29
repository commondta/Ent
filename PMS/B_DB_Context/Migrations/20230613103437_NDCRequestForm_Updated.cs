using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class NDCRequestForm_Updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Block",
                table: "NDCRequestForMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "NDCRequestForMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConstrucationStatus",
                table: "NDCRequestForMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PossessionStatus",
                table: "NDCRequestForMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "NDCRequestForMember",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Block",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "ConstrucationStatus",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "PossessionStatus",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "NDCRequestForMember");
        }
    }
}
