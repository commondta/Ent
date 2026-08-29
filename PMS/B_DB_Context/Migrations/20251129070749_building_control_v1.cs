using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class building_control_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DemarcationFileSubmitedDate",
                table: "StockCreation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChargeTypeId",
                table: "NewDemarcationRequestDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Redesign",
                table: "GlobalChargeSetup",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConstructedStatus",
                table: "ConstructionMonitoring",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DemarcationFileSubmitedDate",
                table: "StockCreation")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "StockCreationHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);

            migrationBuilder.DropColumn(
                name: "ChargeTypeId",
                table: "NewDemarcationRequestDetail")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "NewDemarcationRequestDetailHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);

            migrationBuilder.DropColumn(
                name: "Redesign",
                table: "GlobalChargeSetup")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "GlobalChargeSetupHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);

            migrationBuilder.DropColumn(
                name: "ConstructedStatus",
                table: "ConstructionMonitoring")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "ConstructionMonitoringHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);
        }
    }
}
