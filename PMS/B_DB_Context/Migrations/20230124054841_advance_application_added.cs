using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class advance_application_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvanceApplication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealId = table.Column<int>(type: "int", nullable: true),
                    DealerId = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvanceApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvanceApplication_Deal_DealId",
                        column: x => x.DealId,
                        principalTable: "Deal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvanceApplication_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DealAdvanceApplicationHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutstandingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AmountApplied = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdvanceApplicationId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealAdvanceApplicationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealAdvanceApplicationHistory_AdvanceApplication_AdvanceApplicationId",
                        column: x => x.AdvanceApplicationId,
                        principalTable: "AdvanceApplication",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DealAdvanceApplicationRecipt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReciptNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdvanceApplicationId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealAdvanceApplicationRecipt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealAdvanceApplicationRecipt_AdvanceApplication_AdvanceApplicationId",
                        column: x => x.AdvanceApplicationId,
                        principalTable: "AdvanceApplication",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvanceApplication_DealerId",
                table: "AdvanceApplication",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvanceApplication_DealId",
                table: "AdvanceApplication",
                column: "DealId");

            migrationBuilder.CreateIndex(
                name: "IX_DealAdvanceApplicationHistory_AdvanceApplicationId",
                table: "DealAdvanceApplicationHistory",
                column: "AdvanceApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_DealAdvanceApplicationRecipt_AdvanceApplicationId",
                table: "DealAdvanceApplicationRecipt",
                column: "AdvanceApplicationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DealAdvanceApplicationHistory");

            migrationBuilder.DropTable(
                name: "DealAdvanceApplicationRecipt");

            migrationBuilder.DropTable(
                name: "AdvanceApplication");
        }
    }
}
