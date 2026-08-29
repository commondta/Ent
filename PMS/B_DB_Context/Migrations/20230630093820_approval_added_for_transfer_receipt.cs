using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class approval_added_for_transfer_receipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGovtProcessingTaxApproved",
                table: "TransferHistory",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGovtProcessingTaxRequested",
                table: "TransferHistory",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGovtProcessingTaxApproved",
                table: "TransferHistory");

            migrationBuilder.DropColumn(
                name: "IsGovtProcessingTaxRequested",
                table: "TransferHistory");
        }
    }
}
