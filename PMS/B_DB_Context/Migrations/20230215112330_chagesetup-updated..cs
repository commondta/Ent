using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class chagesetupupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GlobalChargeSetup_Blocks_BlockId",
                table: "GlobalChargeSetup");

            migrationBuilder.DropForeignKey(
                name: "FK_GlobalChargeSetup_Categories_CategoryId",
                table: "GlobalChargeSetup");

            migrationBuilder.DropForeignKey(
                name: "FK_GlobalChargeSetup_Natures_NatureId",
                table: "GlobalChargeSetup");

            migrationBuilder.DropForeignKey(
                name: "FK_GlobalChargeSetup_Phases_PhaseId",
                table: "GlobalChargeSetup");

            migrationBuilder.DropForeignKey(
                name: "FK_GlobalChargeSetup_Projects_ProjectId",
                table: "GlobalChargeSetup");

            migrationBuilder.DropForeignKey(
                name: "FK_GlobalChargeSetup_PropertyTypes_PropertyTypeId",
                table: "GlobalChargeSetup");

            migrationBuilder.DropForeignKey(
                name: "FK_GlobalChargeSetup_Real_Estates_Real_EstateID",
                table: "GlobalChargeSetup");

            migrationBuilder.DropIndex(
                name: "IX_GlobalChargeSetup_BlockId",
                table: "GlobalChargeSetup");

            migrationBuilder.DropIndex(
                name: "IX_GlobalChargeSetup_CategoryId",
                table: "GlobalChargeSetup");

            migrationBuilder.DropIndex(
                name: "IX_GlobalChargeSetup_NatureId",
                table: "GlobalChargeSetup");

            migrationBuilder.DropIndex(
                name: "IX_GlobalChargeSetup_PhaseId",
                table: "GlobalChargeSetup");

            migrationBuilder.DropIndex(
                name: "IX_GlobalChargeSetup_ProjectId",
                table: "GlobalChargeSetup");

            migrationBuilder.DropIndex(
                name: "IX_GlobalChargeSetup_PropertyTypeId",
                table: "GlobalChargeSetup");

            migrationBuilder.DropIndex(
                name: "IX_GlobalChargeSetup_Real_EstateID",
                table: "GlobalChargeSetup");

            migrationBuilder.DropColumn(
                name: "Real_EstateID",
                table: "GlobalChargeSetup");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Real_EstateID",
                table: "GlobalChargeSetup",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_BlockId",
                table: "GlobalChargeSetup",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_CategoryId",
                table: "GlobalChargeSetup",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_NatureId",
                table: "GlobalChargeSetup",
                column: "NatureId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_PhaseId",
                table: "GlobalChargeSetup",
                column: "PhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_ProjectId",
                table: "GlobalChargeSetup",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_PropertyTypeId",
                table: "GlobalChargeSetup",
                column: "PropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_Real_EstateID",
                table: "GlobalChargeSetup",
                column: "Real_EstateID");

            migrationBuilder.AddForeignKey(
                name: "FK_GlobalChargeSetup_Blocks_BlockId",
                table: "GlobalChargeSetup",
                column: "BlockId",
                principalTable: "Blocks",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_GlobalChargeSetup_Categories_CategoryId",
                table: "GlobalChargeSetup",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_GlobalChargeSetup_Natures_NatureId",
                table: "GlobalChargeSetup",
                column: "NatureId",
                principalTable: "Natures",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_GlobalChargeSetup_Phases_PhaseId",
                table: "GlobalChargeSetup",
                column: "PhaseId",
                principalTable: "Phases",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_GlobalChargeSetup_Projects_ProjectId",
                table: "GlobalChargeSetup",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_GlobalChargeSetup_PropertyTypes_PropertyTypeId",
                table: "GlobalChargeSetup",
                column: "PropertyTypeId",
                principalTable: "PropertyTypes",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_GlobalChargeSetup_Real_Estates_Real_EstateID",
                table: "GlobalChargeSetup",
                column: "Real_EstateID",
                principalTable: "Real_Estates",
                principalColumn: "ID");
        }
    }
}
