using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class addingNewColumnsinBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SapAccount",
                table: "BookingProcessingCharges",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookingPaymentScheduleErrorMsg",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookingProcessingChargesErrorMsg",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isBookingPaymentSchedulePostedInSap",
                table: "Booking",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isBookingProcessingChargesPostedInSap",
                table: "Booking",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SapAccount",
                table: "BookingProcessingCharges");

            migrationBuilder.DropColumn(
                name: "BookingPaymentScheduleErrorMsg",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "BookingProcessingChargesErrorMsg",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "isBookingPaymentSchedulePostedInSap",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "isBookingProcessingChargesPostedInSap",
                table: "Booking");
        }
    }
}
