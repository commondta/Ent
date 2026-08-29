using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class ndcMemberSlot_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SlotTime",
                table: "NDCRequestForMember",
                newName: "SlotDate");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "WeekSchedules",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "SlotHour",
                table: "NDCRequestForMember",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SlotMintues",
                table: "NDCRequestForMember",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlotHour",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "SlotMintues",
                table: "NDCRequestForMember");

            migrationBuilder.RenameColumn(
                name: "SlotDate",
                table: "NDCRequestForMember",
                newName: "SlotTime");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "WeekSchedules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
