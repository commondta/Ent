using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class booking_added_jointMem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoiningFeeStatus",
                table: "MemberProfile");

            migrationBuilder.AddColumn<string>(
                name: "CNIC",
                table: "BookingJointMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "BookingJointMember",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BookingJointMember",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CNIC",
                table: "BookingJointMember");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "BookingJointMember");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BookingJointMember");

            migrationBuilder.AddColumn<string>(
                name: "JoiningFeeStatus",
                table: "MemberProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
