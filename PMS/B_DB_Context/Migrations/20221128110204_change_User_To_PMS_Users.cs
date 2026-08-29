using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class change_User_To_PMS_Users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.CreateTable(
                name: "PMSUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMP_CODE = table.Column<int>(type: "int", nullable: false),
                    NIC_NO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMP_FULL_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMP_FATHER_NAM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DESIG_DESC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DEPARTMENT_DESC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SHIFT_DESC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JOINING_DATE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMP_BANK_ACC_NO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PAY_ORG_DESC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PAY_CC_DESC = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMSUser", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PMSUser");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updated_By = table.Column<int>(type: "int", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });
        }
    }
}
