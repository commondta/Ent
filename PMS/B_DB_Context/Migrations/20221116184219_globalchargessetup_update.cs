using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class globalchargessetup_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RealStateTypeId",
                table: "GlobalChargeSetup",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Real_EstateID",
                table: "GlobalChargeSetup",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_Real_EstateID",
                table: "GlobalChargeSetup",
                column: "Real_EstateID");

            migrationBuilder.AddForeignKey(
                name: "FK_GlobalChargeSetup_Real_Estates_Real_EstateID",
                table: "GlobalChargeSetup",
                column: "Real_EstateID",
                principalTable: "Real_Estates",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GlobalChargeSetup_Real_Estates_Real_EstateID",
                table: "GlobalChargeSetup");

            migrationBuilder.DropIndex(
                name: "IX_GlobalChargeSetup_Real_EstateID",
                table: "GlobalChargeSetup");

            migrationBuilder.DropColumn(
                name: "RealStateTypeId",
                table: "GlobalChargeSetup");

            migrationBuilder.DropColumn(
                name: "Real_EstateID",
                table: "GlobalChargeSetup");
        }
    }
}
