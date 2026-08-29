using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class updatingBookingBackLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingProcessingChargePosted",
                table: "BookingBackLog");

            migrationBuilder.RenameColumn(
                name: "BookingSchedulePaymentPlanPosted",
                table: "BookingBackLog",
                newName: "BookingChargePosted");

            migrationBuilder.RenameColumn(
                name: "BookingSchedulePaymentPlanId",
                table: "BookingBackLog",
                newName: "BookingType");

            migrationBuilder.RenameColumn(
                name: "BookingProcessingChargeId",
                table: "BookingBackLog",
                newName: "BookingChargeId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ResolationDate",
                table: "MemberRelationshipHistory",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PassportExpiryDate",
                table: "MemberProfile",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookingType",
                table: "BookingBackLog",
                newName: "BookingSchedulePaymentPlanId");

            migrationBuilder.RenameColumn(
                name: "BookingChargePosted",
                table: "BookingBackLog",
                newName: "BookingSchedulePaymentPlanPosted");

            migrationBuilder.RenameColumn(
                name: "BookingChargeId",
                table: "BookingBackLog",
                newName: "BookingProcessingChargeId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ResolationDate",
                table: "MemberRelationshipHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PassportExpiryDate",
                table: "MemberProfile",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BookingProcessingChargePosted",
                table: "BookingBackLog",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
