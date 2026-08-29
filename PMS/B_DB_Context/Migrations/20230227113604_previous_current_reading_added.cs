using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class previous_current_reading_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentReading",
                table: "MeterBillGenerationDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousReading",
                table: "MeterBillGenerationDetail",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentReading",
                table: "MeterBillGenerationDetail");

            migrationBuilder.DropColumn(
                name: "PreviousReading",
                table: "MeterBillGenerationDetail");
        }
    }
}
