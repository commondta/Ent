using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class partialWHTAX : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WHStatus",
                table: "GlobalChargeDetail",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FixedChargeBillWHApplied",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetAmount = table.Column<int>(type: "int", nullable: false),
                    WHPercentage = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    FixedChargeBillId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixedChargeBillWHApplied", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedChargeBillWHApplied_FixedChargeBill_FixedChargeBillId",
                        column: x => x.FixedChargeBillId,
                        principalTable: "FixedChargeBill",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FixedChargeBillWHApplied_FixedChargeBillId",
                table: "FixedChargeBillWHApplied",
                column: "FixedChargeBillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FixedChargeBillWHApplied");

            migrationBuilder.DropColumn(
                name: "WHStatus",
                table: "GlobalChargeDetail");
        }
    }
}
