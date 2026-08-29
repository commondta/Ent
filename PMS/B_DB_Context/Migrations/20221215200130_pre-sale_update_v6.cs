using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class presale_update_v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DealerProfileId",
                table: "PreSale",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MemberProfileId",
                table: "PreSale",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DealerProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealerStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResidentialAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealerCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstateName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RenewalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutstandingBalance = table.Column<double>(type: "float", nullable: true),
                    OutstandingAdvance = table.Column<double>(type: "float", nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
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
                    TargetPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CopyToTargetDocument = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealerProfileId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
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
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelephoneNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DealerProfileId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
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
                name: "IX_PreSale_DealerProfileId",
                table: "PreSale",
                column: "DealerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_PreSale_MemberProfileId",
                table: "PreSale",
                column: "MemberProfileId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_PreSale_MemberProfile_MemberProfileId",
                table: "PreSale",
                column: "MemberProfileId",
                principalTable: "MemberProfile",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreSale_DealerProfile_DealerProfileId",
                table: "PreSale");

            migrationBuilder.DropForeignKey(
                name: "FK_PreSale_MemberProfile_MemberProfileId",
                table: "PreSale");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "EstateDetail");

            migrationBuilder.DropTable(
                name: "DealerProfile");

            migrationBuilder.DropIndex(
                name: "IX_PreSale_DealerProfileId",
                table: "PreSale");

            migrationBuilder.DropIndex(
                name: "IX_PreSale_MemberProfileId",
                table: "PreSale");

            migrationBuilder.DropColumn(
                name: "DealerProfileId",
                table: "PreSale");

            migrationBuilder.DropColumn(
                name: "MemberProfileId",
                table: "PreSale");
        }
    }
}
