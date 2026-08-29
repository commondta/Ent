using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class bill_generationCheck_added_registrationProfile_SaleTax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "SaleTax",
                table: "FixedChargeBillDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SaleTaxAmount",
                table: "FixedChargeBillDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "SaleTax",
                table: "FixedChargeBillDetail");

            migrationBuilder.DropColumn(
                name: "SaleTaxAmount",
                table: "FixedChargeBillDetail");
        }
    }
}
