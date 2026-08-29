using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class meterphase_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MeterPhaseId",
                table: "MeterDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MeterPhase",
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
                    table.PrimaryKey("PK_MeterPhase", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeterDetail_MeterPhaseId",
                table: "MeterDetail",
                column: "MeterPhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeterDetail_MeterPhase_MeterPhaseId",
                table: "MeterDetail",
                column: "MeterPhaseId",
                principalTable: "MeterPhase",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterDetail_MeterPhase_MeterPhaseId",
                table: "MeterDetail");

            migrationBuilder.DropTable(
                name: "MeterPhase");

            migrationBuilder.DropIndex(
                name: "IX_MeterDetail_MeterPhaseId",
                table: "MeterDetail");

            migrationBuilder.DropColumn(
                name: "MeterPhaseId",
                table: "MeterDetail");
        }
    }
}
