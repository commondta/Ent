using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class SAP_DB_Integration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "IndividualBill");

            migrationBuilder.AddColumn<int>(
                name: "OtherDuesAmount",
                table: "MeterBillGenerationDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OtherDuesDescription",
                table: "MeterBillGenerationDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Surcharge",
                table: "MeterBillGenerationDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GracePeriodSetup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PossessionGracePriod = table.Column<int>(type: "int", nullable: true),
                    TransferGracePeriod = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GracePeriodSetup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SAPBilling",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Server = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DBName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DBUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DBPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SAPUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SAPPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DBType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAPBilling", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SAPOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Server = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DBName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DBUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DBPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SAPUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SAPPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DBType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAPOperations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GracePeriodSetup");

            migrationBuilder.DropTable(
                name: "SAPBilling");

            migrationBuilder.DropTable(
                name: "SAPOperations");

            migrationBuilder.DropColumn(
                name: "OtherDuesAmount",
                table: "MeterBillGenerationDetail");

            migrationBuilder.DropColumn(
                name: "OtherDuesDescription",
                table: "MeterBillGenerationDetail");

            migrationBuilder.DropColumn(
                name: "Surcharge",
                table: "MeterBillGenerationDetail");

            migrationBuilder.AddColumn<int>(
                name: "TotalAmount",
                table: "IndividualBill",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
