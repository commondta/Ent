using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class MemebrProfile_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferedBy",
                table: "PreSale",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MemberProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HonorificsName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Relationship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelationshipWith = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cnic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CnicExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassportNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassportExpiryDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OverSeas = table.Column<bool>(type: "bit", nullable: false),
                    CountryOfResidence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityOfResidence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceOfInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JoiningFeeStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutstandingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NICOPNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    POCNO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BioMetircInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResidenenceStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PermanentAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vehicle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MothersMaidenName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhatsAppNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficeNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImoNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstagramId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkedId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacebookId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TwitterId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profession = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BussinessAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BussinessTenoure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoOfDepartments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelationshipManager = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NTNNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemberAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TargetPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CopyToTargetDocument = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberProfileId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberAttachments_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MemberInterest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyNature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberProfileId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberInterest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberInterest_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MemberRelationshipHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlertNarration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlertType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResolationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResolationDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberProfileId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRelationshipHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberRelationshipHistory_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MemberSocialStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SocialStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberProfileId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberSocialStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberSocialStatus_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberAttachments_MemberProfileId",
                table: "MemberAttachments",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberInterest_MemberProfileId",
                table: "MemberInterest",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRelationshipHistory_MemberProfileId",
                table: "MemberRelationshipHistory",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberSocialStatus_MemberProfileId",
                table: "MemberSocialStatus",
                column: "MemberProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberAttachments");

            migrationBuilder.DropTable(
                name: "MemberInterest");

            migrationBuilder.DropTable(
                name: "MemberRelationshipHistory");

            migrationBuilder.DropTable(
                name: "MemberSocialStatus");

            migrationBuilder.DropTable(
                name: "MemberProfile");

            migrationBuilder.DropColumn(
                name: "ReferedBy",
                table: "PreSale");
        }
    }
}
