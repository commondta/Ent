using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class bc_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OccupationStatus",
                table: "ConstructionMonitoring")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ConstructionMonitoringHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);

            migrationBuilder.RenameColumn(
                name: "Nmae",
                table: "SiteServicesCM",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "ViolationTypeName",
                table: "ViolationGroupType",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "ViolationGroupType",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ViolationGroupType")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ViolationGroupTypeHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SiteServicesCM",
                newName: "Nmae");

            migrationBuilder.AlterColumn<string>(
                name: "ViolationTypeName",
                table: "ViolationGroupType",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "OccupationStatus",
                table: "ConstructionMonitoring",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
