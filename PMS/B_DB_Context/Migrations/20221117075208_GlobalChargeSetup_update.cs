using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class GlobalChargeSetup_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GlobalChargeSetup_BlockId",
                table: "GlobalChargeSetup");

            migrationBuilder.DropIndex(
                name: "IX_GlobalChargeSetup_CategoryId",
                table: "GlobalChargeSetup");

            migrationBuilder.DropIndex(
                name: "IX_GlobalChargeSetup_GlobalChargeGroupId",
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

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_BlockId",
                table: "GlobalChargeSetup",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_CategoryId",
                table: "GlobalChargeSetup",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_GlobalChargeGroupId",
                table: "GlobalChargeSetup",
                column: "GlobalChargeGroupId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GlobalChargeSetup_BlockId",
                table: "GlobalChargeSetup");

            migrationBuilder.DropIndex(
                name: "IX_GlobalChargeSetup_CategoryId",
                table: "GlobalChargeSetup");

            migrationBuilder.DropIndex(
                name: "IX_GlobalChargeSetup_GlobalChargeGroupId",
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

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_BlockId",
                table: "GlobalChargeSetup",
                column: "BlockId",
                unique: true,
                filter: "[BlockId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_CategoryId",
                table: "GlobalChargeSetup",
                column: "CategoryId",
                unique: true,
                filter: "[CategoryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_GlobalChargeGroupId",
                table: "GlobalChargeSetup",
                column: "GlobalChargeGroupId",
                unique: true,
                filter: "[GlobalChargeGroupId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_NatureId",
                table: "GlobalChargeSetup",
                column: "NatureId",
                unique: true,
                filter: "[NatureId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_PhaseId",
                table: "GlobalChargeSetup",
                column: "PhaseId",
                unique: true,
                filter: "[PhaseId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_ProjectId",
                table: "GlobalChargeSetup",
                column: "ProjectId",
                unique: true,
                filter: "[ProjectId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_PropertyTypeId",
                table: "GlobalChargeSetup",
                column: "PropertyTypeId",
                unique: true,
                filter: "[PropertyTypeId] IS NOT NULL");
        }
    }
}
