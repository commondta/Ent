using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class ConstructionSecurity_updated_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConstructionSecurityAttachment_ConstructionSecus_ConstSecuId",
                table: "ConstructionSecurityAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_ConstructionSecurityLabour_ConstructionSecus_ConstSecuId",
                table: "ConstructionSecurityLabour");

            migrationBuilder.DropTable(
                name: "ConstructionSecurityRemark");

            migrationBuilder.DropTable(
                name: "ConstructionSecus");

            migrationBuilder.DropIndex(
                name: "IX_ConstructionSecurityLabour_ConstSecuId",
                table: "ConstructionSecurityLabour");

            migrationBuilder.DropIndex(
                name: "IX_ConstructionSecurityAttachment_ConstSecuId",
                table: "ConstructionSecurityAttachment");

            migrationBuilder.DropColumn(
                name: "ConstSecuId",
                table: "ConstructionSecurityLabour");

            migrationBuilder.DropColumn(
                name: "Created_at",
                table: "ConstructionSecurityLabour");

            migrationBuilder.DropColumn(
                name: "Updated_at",
                table: "ConstructionSecurityLabour");

            migrationBuilder.DropColumn(
                name: "ConstSecuId",
                table: "ConstructionSecurityAttachment");

            migrationBuilder.DropColumn(
                name: "Created_at",
                table: "ConstructionSecurityAttachment");

            migrationBuilder.DropColumn(
                name: "Updated_at",
                table: "ConstructionSecurityAttachment");

            migrationBuilder.RenameColumn(
                name: "CnicAttachment",
                table: "ConstructionSecurityLabour",
                newName: "CNICAttachment");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ConstructionSecurityLabour",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "ConstructionSecurityLabour",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "ConstructionSecurityLabour",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Updated_By",
                table: "ConstructionSecurityLabour",
                newName: "ModifiedBy");

            migrationBuilder.RenameColumn(
                name: "Created_By",
                table: "ConstructionSecurityLabour",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ConstructionSecurityAttachment",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "ConstructionSecurityAttachment",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "ConstructionSecurityAttachment",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Updated_By",
                table: "ConstructionSecurityAttachment",
                newName: "ModifiedBy");

            migrationBuilder.RenameColumn(
                name: "Created_By",
                table: "ConstructionSecurityAttachment",
                newName: "CreatedBy");

            migrationBuilder.AddColumn<bool>(
                name: "Is_ConstructionSecurityApproved",
                table: "StockCreations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Is_ConstructionSecurityRequested",
                table: "StockCreations",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "GatePassValidity",
                table: "ConstructionSecurityLabour",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CNICAttachment",
                table: "ConstructionSecurityLabour",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ConstructionSecurityId",
                table: "ConstructionSecurityLabour",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ConstructionSecurityLabour",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "ConstructionSecurityLabour",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "ConstructionSecurityAttachment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AttachmentDate",
                table: "ConstructionSecurityAttachment",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Attachment",
                table: "ConstructionSecurityAttachment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                table: "ConstructionSecurityAttachment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ConstructionSecurityId",
                table: "ConstructionSecurityAttachment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ConstructionSecurityAttachment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "ConstructionSecurityAttachment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ConstructionSecurity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockCreationId = table.Column<int>(type: "int", nullable: true),
                    ContractorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstructionSecurity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConstructionSecurity_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionSecurityLabour_ConstructionSecurityId",
                table: "ConstructionSecurityLabour",
                column: "ConstructionSecurityId");

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionSecurityAttachment_ConstructionSecurityId",
                table: "ConstructionSecurityAttachment",
                column: "ConstructionSecurityId");

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionSecurity_StockCreationId",
                table: "ConstructionSecurity",
                column: "StockCreationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConstructionSecurityAttachment_ConstructionSecurity_ConstructionSecurityId",
                table: "ConstructionSecurityAttachment",
                column: "ConstructionSecurityId",
                principalTable: "ConstructionSecurity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConstructionSecurityLabour_ConstructionSecurity_ConstructionSecurityId",
                table: "ConstructionSecurityLabour",
                column: "ConstructionSecurityId",
                principalTable: "ConstructionSecurity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConstructionSecurityAttachment_ConstructionSecurity_ConstructionSecurityId",
                table: "ConstructionSecurityAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_ConstructionSecurityLabour_ConstructionSecurity_ConstructionSecurityId",
                table: "ConstructionSecurityLabour");

            migrationBuilder.DropTable(
                name: "ConstructionSecurity");

            migrationBuilder.DropIndex(
                name: "IX_ConstructionSecurityLabour_ConstructionSecurityId",
                table: "ConstructionSecurityLabour");

            migrationBuilder.DropIndex(
                name: "IX_ConstructionSecurityAttachment_ConstructionSecurityId",
                table: "ConstructionSecurityAttachment");

            migrationBuilder.DropColumn(
                name: "Is_ConstructionSecurityApproved",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "Is_ConstructionSecurityRequested",
                table: "StockCreations");

            migrationBuilder.DropColumn(
                name: "ConstructionSecurityId",
                table: "ConstructionSecurityLabour");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ConstructionSecurityLabour");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "ConstructionSecurityLabour");

            migrationBuilder.DropColumn(
                name: "Attachment",
                table: "ConstructionSecurityAttachment");

            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                table: "ConstructionSecurityAttachment");

            migrationBuilder.DropColumn(
                name: "ConstructionSecurityId",
                table: "ConstructionSecurityAttachment");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ConstructionSecurityAttachment");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "ConstructionSecurityAttachment");

            migrationBuilder.RenameColumn(
                name: "CNICAttachment",
                table: "ConstructionSecurityLabour",
                newName: "CnicAttachment");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ConstructionSecurityLabour",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "ConstructionSecurityLabour",
                newName: "Updated_By");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "ConstructionSecurityLabour",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "ConstructionSecurityLabour",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "ConstructionSecurityLabour",
                newName: "Created_By");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ConstructionSecurityAttachment",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "ConstructionSecurityAttachment",
                newName: "Updated_By");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "ConstructionSecurityAttachment",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "ConstructionSecurityAttachment",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "ConstructionSecurityAttachment",
                newName: "Created_By");

            migrationBuilder.AlterColumn<string>(
                name: "GatePassValidity",
                table: "ConstructionSecurityLabour",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CnicAttachment",
                table: "ConstructionSecurityLabour",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConstSecuId",
                table: "ConstructionSecurityLabour",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_at",
                table: "ConstructionSecurityLabour",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated_at",
                table: "ConstructionSecurityLabour",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "ConstructionSecurityAttachment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AttachmentDate",
                table: "ConstructionSecurityAttachment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConstSecuId",
                table: "ConstructionSecurityAttachment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_at",
                table: "ConstructionSecurityAttachment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated_at",
                table: "ConstructionSecurityAttachment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConstructionSecus",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_By = table.Column<int>(type: "int", nullable: true),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updated_By = table.Column<int>(type: "int", nullable: true),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstructionSecus", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ConstructionSecurityRemark",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConstSecuId = table.Column<int>(type: "int", nullable: true),
                    Created_By = table.Column<int>(type: "int", nullable: true),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RemarksBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updated_By = table.Column<int>(type: "int", nullable: true),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstructionSecurityRemark", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ConstructionSecurityRemark_ConstructionSecus_ConstSecuId",
                        column: x => x.ConstSecuId,
                        principalTable: "ConstructionSecus",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionSecurityLabour_ConstSecuId",
                table: "ConstructionSecurityLabour",
                column: "ConstSecuId");

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionSecurityAttachment_ConstSecuId",
                table: "ConstructionSecurityAttachment",
                column: "ConstSecuId");

            migrationBuilder.CreateIndex(
                name: "IX_ConstructionSecurityRemark_ConstSecuId",
                table: "ConstructionSecurityRemark",
                column: "ConstSecuId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConstructionSecurityAttachment_ConstructionSecus_ConstSecuId",
                table: "ConstructionSecurityAttachment",
                column: "ConstSecuId",
                principalTable: "ConstructionSecus",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ConstructionSecurityLabour_ConstructionSecus_ConstSecuId",
                table: "ConstructionSecurityLabour",
                column: "ConstSecuId",
                principalTable: "ConstructionSecus",
                principalColumn: "ID");
        }
    }
}
