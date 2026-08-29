using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class chargessetupchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayableTo",
                table: "GlobalChargeSetup",
                newName: "Description");

            migrationBuilder.AlterColumn<bool>(
                name: "PossessionStatus",
                table: "GlobalChargeSetup",
                type: "bit",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "GlobalChargeSetup",
                newName: "PayableTo");

            migrationBuilder.AlterColumn<string>(
                name: "PossessionStatus",
                table: "GlobalChargeSetup",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
