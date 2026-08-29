using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class softlock_and_alert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_WithHoldingTaxPropertyWise_RegistrationNoProfile_RegistrationNoProfileId",
            //    table: "WithHoldingTaxPropertyWise");

            //migrationBuilder.DropIndex(
            //    name: "IX_WithHoldingTaxPropertyWise_RegistrationNoProfileId",
            //    table: "WithHoldingTaxPropertyWise");

            //migrationBuilder.DropColumn(
            //    name: "RegistrationNoProfileId",
            //    table: "WithHoldingTaxPropertyWise");

            migrationBuilder.DropColumn(
                name: "GeneratorUnitType",
                table: "RegistrationNoProfile");

            migrationBuilder.DropColumn(
                name: "IsBillGenerationEnabled",
                table: "RegistrationNoProfile");

            migrationBuilder.DropColumn(
                name: "IsSaleTaxEnabled",
                table: "RegistrationNoProfile");

            migrationBuilder.DropColumn(
                name: "IsWithHoldingTaxEnabled",
                table: "RegistrationNoProfile");

            migrationBuilder.DropColumn(
                name: "Attachment",
                table: "Alerts");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "SoftLock",
                newName: "SoftLockName");

            migrationBuilder.CreateTable(
                name: "AlertNames",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created_By = table.Column<int>(type: "int", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_By = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertNames", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SoftLockNames",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created_By = table.Column<int>(type: "int", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_By = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftLockNames", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertNames");

            migrationBuilder.DropTable(
                name: "SoftLockNames");

            migrationBuilder.RenameColumn(
                name: "SoftLockName",
                table: "SoftLock",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "RegistrationNoProfileId",
                table: "WithHoldingTaxPropertyWise",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GeneratorUnitType",
                table: "RegistrationNoProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsBillGenerationEnabled",
                table: "RegistrationNoProfile",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSaleTaxEnabled",
                table: "RegistrationNoProfile",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsWithHoldingTaxEnabled",
                table: "RegistrationNoProfile",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Attachment",
                table: "Alerts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            //migrationBuilder.CreateIndex(
            //    name: "IX_WithHoldingTaxPropertyWise_RegistrationNoProfileId",
            //    table: "WithHoldingTaxPropertyWise",
            //    column: "RegistrationNoProfileId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_WithHoldingTaxPropertyWise_RegistrationNoProfile_RegistrationNoProfileId",
            //    table: "WithHoldingTaxPropertyWise",
            //    column: "RegistrationNoProfileId",
            //    principalTable: "RegistrationNoProfile",
            //    principalColumn: "Id");
        }
    }
}
