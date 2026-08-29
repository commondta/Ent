using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class fields_added_in_recordroom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedReceivingDate",
                table: "StoreRoomFileMoving",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecordRoom",
                table: "StoreRoomFileMoving",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PageOutIn",
                table: "StoreRoomFileMoving",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedReceivingDate",
                table: "StoreRoomFileMoving");

            migrationBuilder.DropColumn(
                name: "IsRecordRoom",
                table: "StoreRoomFileMoving");

            migrationBuilder.DropColumn(
                name: "PageOutIn",
                table: "StoreRoomFileMoving");
        }
    }
}
