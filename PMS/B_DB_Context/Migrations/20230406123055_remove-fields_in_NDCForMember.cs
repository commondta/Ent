using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class removefields_in_NDCForMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NDCRequestForMember_NDCRequestType_NDCRequestTypeID",
                table: "NDCRequestForMember");

            migrationBuilder.DropIndex(
                name: "IX_NDCRequestForMember_NDCRequestTypeID",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "NDCRequestTypeID",
                table: "NDCRequestForMember");

            migrationBuilder.AddColumn<string>(
                name: "NDCRequestType",
                table: "NDCRequestForMember",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NDCRequestType",
                table: "NDCRequestForMember");

            migrationBuilder.AddColumn<int>(
                name: "NDCRequestTypeID",
                table: "NDCRequestForMember",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NDCRequestForMember_NDCRequestTypeID",
                table: "NDCRequestForMember",
                column: "NDCRequestTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_NDCRequestForMember_NDCRequestType_NDCRequestTypeID",
                table: "NDCRequestForMember",
                column: "NDCRequestTypeID",
                principalTable: "NDCRequestType",
                principalColumn: "ID");
        }
    }
}
