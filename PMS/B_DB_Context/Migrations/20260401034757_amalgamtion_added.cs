using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class amalgamtion_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amalgamation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockCreationId = table.Column<int>(type: "int", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_Amalgamation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Amalgamation_StockCreation_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreation",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AmalgamationDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockCreationId = table.Column<int>(type: "int", nullable: true),
                    AmalgamationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmalgamationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AmalgamationDetails_Amalgamation_AmalgamationId",
                        column: x => x.AmalgamationId,
                        principalTable: "Amalgamation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AmalgamationDetails_StockCreation_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreation",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amalgamation_StockCreationId",
                table: "Amalgamation",
                column: "StockCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_AmalgamationDetails_AmalgamationId",
                table: "AmalgamationDetails",
                column: "AmalgamationId");

            migrationBuilder.CreateIndex(
                name: "IX_AmalgamationDetails_StockCreationId",
                table: "AmalgamationDetails",
                column: "StockCreationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AmalgamationDetails");

            migrationBuilder.DropTable(
                name: "Amalgamation");
        }
    }
}
