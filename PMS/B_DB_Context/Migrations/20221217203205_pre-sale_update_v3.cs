using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class presale_update_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreSale_DealerProfile_DealerProfileId",
                table: "PreSale");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "EstateDetail");

            migrationBuilder.DropTable(
                name: "DealerProfile");

            migrationBuilder.DropColumn(
                name: "NetTotal",
                table: "PaymentPlan");

            migrationBuilder.RenameColumn(
                name: "DealerProfileId",
                table: "PreSale",
                newName: "DealerId");

            migrationBuilder.RenameIndex(
                name: "IX_PreSale_DealerProfileId",
                table: "PreSale",
                newName: "IX_PreSale_DealerId");

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentPlanTotal",
                table: "PreSale",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_PreSale_Dealers_DealerId",
                table: "PreSale",
                column: "DealerId",
                principalTable: "Dealers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreSale_Dealers_DealerId",
                table: "PreSale");

            migrationBuilder.DropColumn(
                name: "PaymentPlanTotal",
                table: "PreSale");

            migrationBuilder.RenameColumn(
                name: "DealerId",
                table: "PreSale",
                newName: "DealerProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_PreSale_DealerId",
                table: "PreSale",
                newName: "IX_PreSale_DealerProfileId");

            migrationBuilder.AddColumn<decimal>(
                name: "NetTotal",
                table: "PaymentPlan",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "DealerProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DealerCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealerCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealerStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstateName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutstandingAdvance = table.Column<double>(type: "float", nullable: true),
                    OutstandingBalance = table.Column<double>(type: "float", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RenewalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResidentialAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerProfileId = table.Column<int>(type: "int", nullable: true),
                    AttachmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CopyToTargetDocument = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    TargetPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_DealerProfile_DealerProfileId",
                        column: x => x.DealerProfileId,
                        principalTable: "DealerProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EstateDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerProfileId = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelephoneNo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstateDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstateDetail_DealerProfile_DealerProfileId",
                        column: x => x.DealerProfileId,
                        principalTable: "DealerProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_DealerProfileId",
                table: "Attachments",
                column: "DealerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_EstateDetail_DealerProfileId",
                table: "EstateDetail",
                column: "DealerProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreSale_DealerProfile_DealerProfileId",
                table: "PreSale",
                column: "DealerProfileId",
                principalTable: "DealerProfile",
                principalColumn: "Id");
        }
    }
}
