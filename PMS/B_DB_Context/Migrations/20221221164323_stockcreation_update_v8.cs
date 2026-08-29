using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class stockcreation_update_v8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DealerId",
                table: "StockCreations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MemberProfileId",
                table: "StockCreations",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DoucmentName",
                table: "NDCRequestForMemberAttachments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Document",
                table: "NDCRequestForMemberAttachments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "NDCRequestForMemberAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockCreations_DealerId",
                table: "StockCreations",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_StockCreations_MemberProfileId",
                table: "StockCreations",
                column: "MemberProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockCreations_Dealers_DealerId",
                table: "StockCreations",
                column: "DealerId",
                principalTable: "Dealers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockCreations_MemberProfile_MemberProfileId",
                table: "StockCreations",
                column: "MemberProfileId",
                principalTable: "MemberProfile",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockCreations_Dealers_DealerId",
                table: "StockCreations");

            migrationBuilder.DropForeignKey(
                name: "FK_StockCreations_MemberProfile_MemberProfileId",
                table: "StockCreations");

            migrationBuilder.DropIndex(
                name: "IX_StockCreations_DealerId",
                table: "StockCreations");

            migrationBuilder.DropIndex(
                name: "IX_StockCreations_MemberProfileId",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "DealerId",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "MemberProfileId",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "NDCRequestForMemberAttachments");

            migrationBuilder.AlterColumn<string>(
                name: "DoucmentName",
                table: "NDCRequestForMemberAttachments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Document",
                table: "NDCRequestForMemberAttachments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
