using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class surrender_update_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSurrenderApproved",
                table: "Surrender",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSurrenderRequest",
                table: "Surrender",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Surrender",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsReSurrenderApproved",
                table: "Resurrender",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReSurrenderRequest",
                table: "Resurrender",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Resurrender",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSurrenderApproved",
                table: "Surrender");

            migrationBuilder.DropColumn(
                name: "IsSurrenderRequest",
                table: "Surrender");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Surrender");

            migrationBuilder.DropColumn(
                name: "IsReSurrenderApproved",
                table: "Resurrender");

            migrationBuilder.DropColumn(
                name: "IsReSurrenderRequest",
                table: "Resurrender");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Resurrender");
        }
    }
}
