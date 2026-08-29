using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class mappApprovedHistory_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is_PossessionApproved",
                table: "StockCreations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Is_PossessionRequested",
                table: "StockCreations",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MapApprovalHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DesignName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Architecture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateofSubmission = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateofFeedback = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Attachments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockCreationID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapApprovalHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapApprovalHistory_StockCreations_StockCreationID",
                        column: x => x.StockCreationID,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapApprovalHistory_StockCreationID",
                table: "MapApprovalHistory",
                column: "StockCreationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapApprovalHistory");

            migrationBuilder.DropColumn(
                name: "Is_PossessionApproved",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "Is_PossessionRequested",
                table: "StockCreations");
        }
    }
}
