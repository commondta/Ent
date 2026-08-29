using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class update_dealerModal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Designation",
                table: "dealerEstateDeatails");

            migrationBuilder.AddColumn<int>(
                name: "DealerDesignationId",
                table: "dealerEstateDeatails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "dealerEstateDeatails",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_dealerEstateDeatails_DealerDesignationId",
                table: "dealerEstateDeatails",
                column: "DealerDesignationId");

            migrationBuilder.AddForeignKey(
                name: "FK_dealerEstateDeatails_DealerDesignation_DealerDesignationId",
                table: "dealerEstateDeatails",
                column: "DealerDesignationId",
                principalTable: "DealerDesignation",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dealerEstateDeatails_DealerDesignation_DealerDesignationId",
                table: "dealerEstateDeatails");

            migrationBuilder.DropIndex(
                name: "IX_dealerEstateDeatails_DealerDesignationId",
                table: "dealerEstateDeatails");

            migrationBuilder.DropColumn(
                name: "DealerDesignationId",
                table: "dealerEstateDeatails");

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "dealerEstateDeatails");

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "dealerEstateDeatails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
