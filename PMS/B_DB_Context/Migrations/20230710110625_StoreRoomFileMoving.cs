using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class StoreRoomFileMoving : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreRoomFileMoving",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockCreationId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    PMSUserId = table.Column<int>(type: "int", nullable: false),
                    IsFileClosed = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    LastModifiedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreRoomFileMoving", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreRoomFileMoving_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreRoomFileMoving_PMSUser_PMSUserId",
                        column: x => x.PMSUserId,
                        principalTable: "PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreRoomFileMoving_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreRoomFileMoving_DepartmentId",
                table: "StoreRoomFileMoving",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreRoomFileMoving_PMSUserId",
                table: "StoreRoomFileMoving",
                column: "PMSUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreRoomFileMoving_StockCreationId",
                table: "StoreRoomFileMoving",
                column: "StockCreationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreRoomFileMoving");
        }
    }
}
