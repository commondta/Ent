using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class defination_forms_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstallationDate",
                table: "MeterInstallation");

            migrationBuilder.RenameColumn(
                name: "TotalUnits",
                table: "ReadingDetail",
                newName: "UnitsConsumed");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentReading",
                table: "ReadingDetail",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LastReading",
                table: "ReadingDetail",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ReadingOfficerId",
                table: "ReadingDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "MeterDetail",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "MeterStatusId",
                table: "MeterDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MapDesigns",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created_By = table.Column<int>(type: "int", nullable: true),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_By = table.Column<int>(type: "int", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapDesigns", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MeterStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReadingOfficer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingOfficer", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReadingDetail_ReadingOfficerId",
                table: "ReadingDetail",
                column: "ReadingOfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterDetail_MeterStatusId",
                table: "MeterDetail",
                column: "MeterStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeterDetail_MeterStatus_MeterStatusId",
                table: "MeterDetail",
                column: "MeterStatusId",
                principalTable: "MeterStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingDetail_ReadingOfficer_ReadingOfficerId",
                table: "ReadingDetail",
                column: "ReadingOfficerId",
                principalTable: "ReadingOfficer",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterDetail_MeterStatus_MeterStatusId",
                table: "MeterDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadingDetail_ReadingOfficer_ReadingOfficerId",
                table: "ReadingDetail");

            migrationBuilder.DropTable(
                name: "MapDesigns");

            migrationBuilder.DropTable(
                name: "MeterStatus");

            migrationBuilder.DropTable(
                name: "ReadingOfficer");

            migrationBuilder.DropIndex(
                name: "IX_ReadingDetail_ReadingOfficerId",
                table: "ReadingDetail");

            migrationBuilder.DropIndex(
                name: "IX_MeterDetail_MeterStatusId",
                table: "MeterDetail");

            migrationBuilder.DropColumn(
                name: "CurrentReading",
                table: "ReadingDetail");

            migrationBuilder.DropColumn(
                name: "LastReading",
                table: "ReadingDetail");

            migrationBuilder.DropColumn(
                name: "ReadingOfficerId",
                table: "ReadingDetail");

            migrationBuilder.DropColumn(
                name: "MeterStatusId",
                table: "MeterDetail");

            migrationBuilder.RenameColumn(
                name: "UnitsConsumed",
                table: "ReadingDetail",
                newName: "TotalUnits");

            migrationBuilder.AddColumn<DateTime>(
                name: "InstallationDate",
                table: "MeterInstallation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "MeterDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
