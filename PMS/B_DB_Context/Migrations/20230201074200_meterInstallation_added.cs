using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class meterInstallation_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeterInstallation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Project = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstallationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockCreationId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterInstallation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeterInstallation_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "MeterDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeterNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitsAtInstallation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeterTypeId = table.Column<int>(type: "int", nullable: true),
                    MeterInstallationId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeterDetail_MeterInstallation_MeterInstallationId",
                        column: x => x.MeterInstallationId,
                        principalTable: "MeterInstallation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MeterDetail_MeterType_MeterTypeId",
                        column: x => x.MeterTypeId,
                        principalTable: "MeterType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeterDetail_MeterInstallationId",
                table: "MeterDetail",
                column: "MeterInstallationId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterDetail_MeterTypeId",
                table: "MeterDetail",
                column: "MeterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterInstallation_StockCreationId",
                table: "MeterInstallation",
                column: "StockCreationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeterDetail");

            migrationBuilder.DropTable(
                name: "MeterInstallation");
        }
    }
}
