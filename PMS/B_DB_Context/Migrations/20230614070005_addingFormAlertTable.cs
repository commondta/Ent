using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class addingFormAlertTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "Block",
            //    table: "NDCRequestForMember",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Category",
            //    table: "NDCRequestForMember",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "ConstrucationStatus",
            //    table: "NDCRequestForMember",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "PossessionStatus",
            //    table: "NDCRequestForMember",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Size",
            //    table: "NDCRequestForMember",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "NDCProcessing",
            //    table: "GlobalChargeSetup",
            //    type: "bit",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "NDCRequestType",
            //    table: "GlobalChargeSetup",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "NDCTransferType",
            //    table: "GlobalChargeSetup",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "FormAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormAlerts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormAlertUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormAlertId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserDesignation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormAlertUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormAlertUsers_FormAlerts_FormAlertId",
                        column: x => x.FormAlertId,
                        principalTable: "FormAlerts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormAlertUsers_FormAlertId",
                table: "FormAlertUsers",
                column: "FormAlertId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormAlertUsers");

            migrationBuilder.DropTable(
                name: "FormAlerts");

            migrationBuilder.DropColumn(
                name: "Block",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "ConstrucationStatus",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "PossessionStatus",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "NDCProcessing",
                table: "GlobalChargeSetup");

            migrationBuilder.DropColumn(
                name: "NDCRequestType",
                table: "GlobalChargeSetup");

            migrationBuilder.DropColumn(
                name: "NDCTransferType",
                table: "GlobalChargeSetup");
        }
    }
}
