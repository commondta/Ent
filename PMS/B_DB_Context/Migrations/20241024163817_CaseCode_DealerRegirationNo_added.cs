using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class CaseCode_DealerRegirationNo_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CaseCode",
                table: "StockCreation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DelaerRegisrationCode",
                table: "Dealer",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaseCode",
                table: "StockCreation")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "StockCreationHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);

            migrationBuilder.DropColumn(
                name: "DelaerRegisrationCode",
                table: "Dealer")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "DealerHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);
        }
    }
}
