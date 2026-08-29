using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class ConstructionMonitoring_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConstructionMonitoringChild");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConstructionMonitorings",
                table: "ConstructionMonitorings");

            migrationBuilder.DropColumn(
                name: "Block",
                table: "ConstructionMonitorings");

            migrationBuilder.DropColumn(
                name: "Created_By",
                table: "ConstructionMonitorings");

            migrationBuilder.DropColumn(
                name: "InspectorName",
                table: "ConstructionMonitorings");

            migrationBuilder.DropColumn(
                name: "MemberCode",
                table: "ConstructionMonitorings");

            migrationBuilder.DropColumn(
                name: "MemberName",
                table: "ConstructionMonitorings");

            migrationBuilder.DropColumn(
                name: "Phase",
                table: "ConstructionMonitorings");

            migrationBuilder.DropColumn(
                name: "PropertyNumber",
                table: "ConstructionMonitorings");

            migrationBuilder.DropColumn(
                name: "RegistrationNumber",
                table: "ConstructionMonitorings");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "ConstructionMonitorings");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "ConstructionMonitorings");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ConstructionMonitorings");

            migrationBuilder.DropColumn(
                name: "Updated_By",
                table: "ConstructionMonitorings");

            migrationBuilder.RenameTable(
                name: "ConstructionMonitorings",
                newName: "ConstructionMonitoring");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ConstructionMonitoring",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "ConstructionMonitoring",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "ConstructionMonitoring",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Updated_at",
                table: "ConstructionMonitoring",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "Created_at",
                table: "ConstructionMonitoring",
                newName: "CreatedOn");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "ConstructionMonitoring",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "ConstructionMonitoring",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockCreationId",
                table: "ConstructionMonitoring",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ConstructionMonitoring",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConstructionMonitoring",
                table: "ConstructionMonitoring",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ConstructionMonitoringStageDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StageCode = table.Column<int>(type: "int", nullable: true),
                    StageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspectionBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Violation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConstrationMonitoringId = table.Column<int>(type: "int", nullable: true),
                    ConstructionMonitoringId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstructionMonitoringStageDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConstructionMonitoringStageDetail_ConstructionMonitoring_ConstructionMonitoringId",
                        column: x => x.ConstructionMonitoringId,
                        principalTable: "ConstructionMonitoring",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionMonitoring_StockCreationId",
                table: "ConstructionMonitoring",
                column: "StockCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionMonitoringStageDetail_ConstructionMonitoringId",
                table: "ConstructionMonitoringStageDetail",
                column: "ConstructionMonitoringId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConstructionMonitoring_StockCreations_StockCreationId",
                table: "ConstructionMonitoring",
                column: "StockCreationId",
                principalTable: "StockCreations",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConstructionMonitoring_StockCreations_StockCreationId",
                table: "ConstructionMonitoring");

            migrationBuilder.DropTable(
                name: "ConstructionMonitoringStageDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConstructionMonitoring",
                table: "ConstructionMonitoring");

            migrationBuilder.DropIndex(
                name: "IX_ConstructionMonitoring_StockCreationId",
                table: "ConstructionMonitoring");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ConstructionMonitoring");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "ConstructionMonitoring");

            migrationBuilder.DropColumn(
                name: "StockCreationId",
                table: "ConstructionMonitoring");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ConstructionMonitoring");

            migrationBuilder.RenameTable(
                name: "ConstructionMonitoring",
                newName: "ConstructionMonitorings");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ConstructionMonitorings",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "ConstructionMonitorings",
                newName: "Updated_at");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "ConstructionMonitorings",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "ConstructionMonitorings",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "ConstructionMonitorings",
                newName: "Created_at");

            migrationBuilder.AddColumn<string>(
                name: "Block",
                table: "ConstructionMonitorings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Created_By",
                table: "ConstructionMonitorings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InspectorName",
                table: "ConstructionMonitorings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberCode",
                table: "ConstructionMonitorings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberName",
                table: "ConstructionMonitorings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phase",
                table: "ConstructionMonitorings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropertyNumber",
                table: "ConstructionMonitorings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNumber",
                table: "ConstructionMonitorings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "ConstructionMonitorings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "ConstructionMonitorings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ConstructionMonitorings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated_By",
                table: "ConstructionMonitorings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConstructionMonitorings",
                table: "ConstructionMonitorings",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "ConstructionMonitoringChild",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConstructionMonitoringID = table.Column<int>(type: "int", nullable: true),
                    ConstrationMonitoringId = table.Column<int>(type: "int", nullable: true),
                    Created_By = table.Column<int>(type: "int", nullable: false),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InspectionBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updated_By = table.Column<int>(type: "int", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Violation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstructionMonitoringChild", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ConstructionMonitoringChild_ConstructionMonitorings_ConstructionMonitoringID",
                        column: x => x.ConstructionMonitoringID,
                        principalTable: "ConstructionMonitorings",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionMonitoringChild_ConstructionMonitoringID",
                table: "ConstructionMonitoringChild",
                column: "ConstructionMonitoringID");
        }
    }
}
