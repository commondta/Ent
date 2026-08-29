using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class renumberHistory_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RenumberHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentPropertyRegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentPropertyPropertyNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentPropertyCNIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentPropertyMemberName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentPropertyBlock = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentPropertyCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentPropertySize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProposedPropertyPropertyNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProposedPropertyBlock = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProposedPropertyCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProposedPropertySize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    LastModifiedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenumberHistories", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RenumberHistories");
        }
    }
}
