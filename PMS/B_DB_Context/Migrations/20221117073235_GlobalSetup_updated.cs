using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class GlobalSetup_updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChargeGroupType_GlobalChargeGroupId",
                table: "ChargeGroupType");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeGroupType_GlobalChargeGroupId",
                table: "ChargeGroupType",
                column: "GlobalChargeGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChargeGroupType_GlobalChargeGroupId",
                table: "ChargeGroupType");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeGroupType_GlobalChargeGroupId",
                table: "ChargeGroupType",
                column: "GlobalChargeGroupId",
                unique: true,
                filter: "[GlobalChargeGroupId] IS NOT NULL");
        }
    }
}
