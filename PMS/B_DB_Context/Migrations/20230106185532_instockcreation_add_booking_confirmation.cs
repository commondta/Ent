using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class instockcreation_add_booking_confirmation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBookingApproved",
                table: "StockCreations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBookingRequested",
                table: "StockCreations",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBookingApproved",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "IsBookingRequested",
                table: "StockCreations");
        }
    }
}
