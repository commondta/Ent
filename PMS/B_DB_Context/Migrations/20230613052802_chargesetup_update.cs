using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class chargesetup_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NDCProcessing",
                table: "GlobalChargeSetup",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NDCRequestType",
                table: "GlobalChargeSetup",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NDCTransferType",
                table: "GlobalChargeSetup",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NDCProcessing",
                table: "GlobalChargeSetup");

            migrationBuilder.DropColumn(
                name: "NDCRequestType",
                table: "GlobalChargeSetup");

            migrationBuilder.DropColumn(
                name: "NDCTransferType",
                table: "GlobalChargeSetup");
        }
    }
}
