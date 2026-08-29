using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class WithHoldingTaxPropertyWise_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WithHoldingTaxPropertyWise",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rate = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: true),
                    StockCreationId = table.Column<int>(type: "int", nullable: true),
                    RegistrationNoProfileId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithHoldingTaxPropertyWise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WithHoldingTaxPropertyWise_RegistrationNoProfile_RegistrationNoProfileId",
                        column: x => x.RegistrationNoProfileId,
                        principalTable: "RegistrationNoProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WithHoldingTaxPropertyWise_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WithHoldingTaxPropertyWise_RegistrationNoProfileId",
                table: "WithHoldingTaxPropertyWise",
                column: "RegistrationNoProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_WithHoldingTaxPropertyWise_StockCreationId",
                table: "WithHoldingTaxPropertyWise",
                column: "StockCreationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WithHoldingTaxPropertyWise");
        }
    }
}
