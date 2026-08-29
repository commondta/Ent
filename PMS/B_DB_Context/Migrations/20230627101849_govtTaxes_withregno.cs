using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class govtTaxes_withregno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RealStateTypeName",
                table: "TransferReceiptProcessing",
                newName: "RegistrationNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegistrationNo",
                table: "TransferReceiptProcessing",
                newName: "RealStateTypeName");
        }
    }
}
