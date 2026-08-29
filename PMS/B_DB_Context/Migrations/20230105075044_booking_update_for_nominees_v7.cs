using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class booking_update_for_nominees_v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingNominee_MemberProfile_MemberProfileId",
                table: "BookingNominee");

            migrationBuilder.DropIndex(
                name: "IX_BookingNominee_MemberProfileId",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "MemberProfileId",
                table: "BookingNominee");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "BookingNominee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CNIC",
                table: "BookingNominee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "BookingNominee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BookingNominee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Relationship",
                table: "BookingNominee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "CNIC",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "Relationship",
                table: "BookingNominee");

            migrationBuilder.AddColumn<int>(
                name: "MemberProfileId",
                table: "BookingNominee",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingNominee_MemberProfileId",
                table: "BookingNominee",
                column: "MemberProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingNominee_MemberProfile_MemberProfileId",
                table: "BookingNominee",
                column: "MemberProfileId",
                principalTable: "MemberProfile",
                principalColumn: "Id");
        }
    }
}
