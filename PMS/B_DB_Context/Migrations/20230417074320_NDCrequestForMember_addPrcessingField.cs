using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class NDCrequestForMember_addPrcessingField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerSeries",
                table: "SAPOperations");

            migrationBuilder.DropColumn(
                name: "DealerAccountCode",
                table: "SAPOperations");

            migrationBuilder.DropColumn(
                name: "MemberAccountCode",
                table: "SAPOperations");

            migrationBuilder.AddColumn<bool>(
                name: "Processing",
                table: "NDCRequestForMember",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Processing",
                table: "NDCRequestForMember");

            migrationBuilder.AddColumn<string>(
                name: "CustomerSeries",
                table: "SAPOperations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DealerAccountCode",
                table: "SAPOperations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MemberAccountCode",
                table: "SAPOperations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
