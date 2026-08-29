using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class addmarketavalue_in_cop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentPropertyMarketValue",
                table: "COPHistories",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProposedPropertyMarketValue",
                table: "COPHistories",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPropertyMarketValue",
                table: "COPHistories");

            migrationBuilder.DropColumn(
                name: "ProposedPropertyMarketValue",
                table: "COPHistories");
        }
    }
}
