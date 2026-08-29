using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class add_propertyNo_meterreading : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PropertyNo",
                table: "ReadingDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSurrenderRequested",
                table: "NDCRequestForMember",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropertyNo",
                table: "ReadingDetail");

            migrationBuilder.DropColumn(
                name: "IsSurrenderRequested",
                table: "NDCRequestForMember");
        }
    }
}
