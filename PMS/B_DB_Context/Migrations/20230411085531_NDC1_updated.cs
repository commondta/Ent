using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class NDC1_updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "NDCRequestForMember",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "NDC1Attachments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "NDC1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DealerCode",
                table: "NDC1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DealerName",
                table: "NDC1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "NDC1",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNDC1Approved",
                table: "NDC1",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNDC1Requested",
                table: "NDC1",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MemberProfileId",
                table: "NDC1",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NDCRequestType",
                table: "NDC1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Outstation",
                table: "NDC1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "NDC1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slot",
                table: "NDC1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SlotDate",
                table: "NDC1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransferType",
                table: "NDC1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValidityDate",
                table: "NDC1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NDC1CheckLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlertNarration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NDC1Id = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NDC1CheckLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NDC1CheckLists_NDC1_NDC1Id",
                        column: x => x.NDC1Id,
                        principalTable: "NDC1",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NDC1_MemberProfileId",
                table: "NDC1",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_NDC1CheckLists_NDC1Id",
                table: "NDC1CheckLists",
                column: "NDC1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NDC1_MemberProfile_MemberProfileId",
                table: "NDC1",
                column: "MemberProfileId",
                principalTable: "MemberProfile",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NDC1_MemberProfile_MemberProfileId",
                table: "NDC1");

            migrationBuilder.DropTable(
                name: "NDC1CheckLists");

            migrationBuilder.DropIndex(
                name: "IX_NDC1_MemberProfileId",
                table: "NDC1");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "NDCRequestForMember");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "NDC1Attachments");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "NDC1");

            migrationBuilder.DropColumn(
                name: "DealerCode",
                table: "NDC1");

            migrationBuilder.DropColumn(
                name: "DealerName",
                table: "NDC1");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "NDC1");

            migrationBuilder.DropColumn(
                name: "IsNDC1Approved",
                table: "NDC1");

            migrationBuilder.DropColumn(
                name: "IsNDC1Requested",
                table: "NDC1");

            migrationBuilder.DropColumn(
                name: "MemberProfileId",
                table: "NDC1");

            migrationBuilder.DropColumn(
                name: "NDCRequestType",
                table: "NDC1");

            migrationBuilder.DropColumn(
                name: "Outstation",
                table: "NDC1");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "NDC1");

            migrationBuilder.DropColumn(
                name: "Slot",
                table: "NDC1");

            migrationBuilder.DropColumn(
                name: "SlotDate",
                table: "NDC1");

            migrationBuilder.DropColumn(
                name: "TransferType",
                table: "NDC1");

            migrationBuilder.DropColumn(
                name: "ValidityDate",
                table: "NDC1");
        }
    }
}
