using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class updatecmfieldId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConstrationMonitoringId",
                table: "ConstructionMonitoringStageDetail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConstrationMonitoringId",
                table: "ConstructionMonitoringStageDetail",
                type: "int",
                nullable: true);
        }
    }
}
