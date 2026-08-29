using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class Dealer_update_v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DealerLoginDetails");

            migrationBuilder.DropColumn(
                name: "DealerCode",
                table: "Dealers");

            migrationBuilder.RenameColumn(
                name: "SerialNo",
                table: "Dealers",
                newName: "DealerStatus");

            migrationBuilder.RenameColumn(
                name: "DocumentStatus",
                table: "Dealers",
                newName: "Password");

            migrationBuilder.AddColumn<double>(
                name: "OutstandingAdvance",
                table: "Dealers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OutstandingBalance",
                table: "Dealers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Dealers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutstandingAdvance",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "OutstandingBalance",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Dealers");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Dealers",
                newName: "DocumentStatus");

            migrationBuilder.RenameColumn(
                name: "DealerStatus",
                table: "Dealers",
                newName: "SerialNo");

            migrationBuilder.AddColumn<string>(
                name: "DealerCode",
                table: "Dealers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DealerLoginDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerLoginDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealerLoginDetails_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DealerLoginDetails_DealerId",
                table: "DealerLoginDetails",
                column: "DealerId",
                unique: true,
                filter: "[DealerId] IS NOT NULL");
        }
    }
}
