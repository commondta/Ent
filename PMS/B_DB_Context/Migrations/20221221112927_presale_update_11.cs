using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class presale_update_11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPreSaleApproved",
                table: "PreSale",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPreSaleRequested",
                table: "PreSale",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPreSaleApproved",
                table: "PreSale");

            migrationBuilder.DropColumn(
                name: "IsPreSaleRequested",
                table: "PreSale");
        }
    }
}
