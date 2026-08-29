using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class Demarcationupdatev1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemarcationDetail");

            migrationBuilder.DropTable(
                name: "DemarcationRequestCharge");

            migrationBuilder.DropTable(
                name: "DemarcationRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Demarcations",
                table: "Demarcations");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "Demarcations");

            migrationBuilder.DropColumn(
                name: "ChargeCleareance",
                table: "Demarcations");

            migrationBuilder.DropColumn(
                name: "Created_at",
                table: "Demarcations");

            migrationBuilder.DropColumn(
                name: "DateOfMapApproval",
                table: "Demarcations");

            migrationBuilder.DropColumn(
                name: "DateofDemarcation",
                table: "Demarcations");

            migrationBuilder.DropColumn(
                name: "DemarcationRequestDate",
                table: "Demarcations");

            migrationBuilder.DropColumn(
                name: "DemarcationRequestNo",
                table: "Demarcations");

            migrationBuilder.DropColumn(
                name: "DocumentDate",
                table: "Demarcations");

            migrationBuilder.DropColumn(
                name: "ExpiryOfGacePeriod",
                table: "Demarcations");

            migrationBuilder.DropColumn(
                name: "GacePeriod",
                table: "Demarcations");

            migrationBuilder.DropColumn(
                name: "MemberCode",
                table: "Demarcations");

            migrationBuilder.DropColumn(
                name: "MemberName",
                table: "Demarcations");

            migrationBuilder.DropColumn(
                name: "PropertyNo",
                table: "Demarcations");

            migrationBuilder.DropColumn(
                name: "RegistrationNo",
                table: "Demarcations");

            migrationBuilder.DropColumn(
                name: "Updated_at",
                table: "Demarcations");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "Demarcations");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "Demarcations");

            migrationBuilder.RenameTable(
                name: "Demarcations",
                newName: "Demarcation");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Demarcation",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "LDAPlotNo",
                table: "Demarcation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Demarcation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Demarcation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Demarcation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Demarcation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Demarcation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "Demarcation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockCreationId",
                table: "Demarcation",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Demarcation",
                table: "Demarcation",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DemarcationFormAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Piture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DemarcationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemarcationFormAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemarcationFormAttachments_Demarcation_DemarcationId",
                        column: x => x.DemarcationId,
                        principalTable: "Demarcation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Demarcation_StockCreationId",
                table: "Demarcation",
                column: "StockCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_DemarcationFormAttachments_DemarcationId",
                table: "DemarcationFormAttachments",
                column: "DemarcationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Demarcation_StockCreations_StockCreationId",
                table: "Demarcation",
                column: "StockCreationId",
                principalTable: "StockCreations",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Demarcation_StockCreations_StockCreationId",
                table: "Demarcation");

            migrationBuilder.DropTable(
                name: "DemarcationFormAttachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Demarcation",
                table: "Demarcation");

            migrationBuilder.DropIndex(
                name: "IX_Demarcation_StockCreationId",
                table: "Demarcation");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Demarcation");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Demarcation");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Demarcation");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Demarcation");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Demarcation");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Demarcation");

            migrationBuilder.DropColumn(
                name: "StockCreationId",
                table: "Demarcation");

            migrationBuilder.RenameTable(
                name: "Demarcation",
                newName: "Demarcations");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Demarcations",
                newName: "ID");

            migrationBuilder.AlterColumn<string>(
                name: "LDAPlotNo",
                table: "Demarcations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "Demarcations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChargeCleareance",
                table: "Demarcations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_at",
                table: "Demarcations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfMapApproval",
                table: "Demarcations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateofDemarcation",
                table: "Demarcations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DemarcationRequestDate",
                table: "Demarcations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DemarcationRequestNo",
                table: "Demarcations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DocumentDate",
                table: "Demarcations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryOfGacePeriod",
                table: "Demarcations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GacePeriod",
                table: "Demarcations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberCode",
                table: "Demarcations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberName",
                table: "Demarcations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropertyNo",
                table: "Demarcations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNo",
                table: "Demarcations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated_at",
                table: "Demarcations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "Demarcations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "Demarcations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Demarcations",
                table: "Demarcations",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "DemarcationDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemarcationId = table.Column<int>(type: "int", nullable: true),
                    Pic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemarcationDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemarcationDetail_Demarcations_DemarcationId",
                        column: x => x.DemarcationId,
                        principalTable: "Demarcations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "DemarcationRequests",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Block = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentNo = table.Column<int>(type: "int", nullable: true),
                    MemberCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemarcationRequests", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DemarcationRequestCharge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemarcationRequestId = table.Column<int>(type: "int", nullable: true),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    chargeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemarcationRequestCharge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemarcationRequestCharge_DemarcationRequests_DemarcationRequestId",
                        column: x => x.DemarcationRequestId,
                        principalTable: "DemarcationRequests",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemarcationDetail_DemarcationId",
                table: "DemarcationDetail",
                column: "DemarcationId");

            migrationBuilder.CreateIndex(
                name: "IX_DemarcationRequestCharge_DemarcationRequestId",
                table: "DemarcationRequestCharge",
                column: "DemarcationRequestId");
        }
    }
}
