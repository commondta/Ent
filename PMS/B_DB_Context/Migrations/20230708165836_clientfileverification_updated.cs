using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class clientfileverification_updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceivedDate",
                table: "ClientFileVerification",
                newName: "RequestType");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "ClientFileVerification",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "ClientFileVerification");

            migrationBuilder.RenameColumn(
                name: "RequestType",
                table: "ClientFileVerification",
                newName: "ReceivedDate");
        }
    }
}
