using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class caseProfile_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaseProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseFor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LandArea = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FIRReferenceNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdvanceDeposit = table.Column<int>(type: "int", nullable: false),
                    SettlementMark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceOfSettlement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermsAndConditionsOfLawyer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LawyerFee = table.Column<int>(type: "int", nullable: false),
                    CourtFee = table.Column<int>(type: "int", nullable: false),
                    Outcome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCaseProfileRequested = table.Column<bool>(type: "bit", nullable: true),
                    IsCaseProfileApproved = table.Column<bool>(type: "bit", nullable: true),
                    StockCreationId = table.Column<int>(type: "int", nullable: true),
                    CaseTypeId = table.Column<int>(type: "int", nullable: true),
                    CaseCategoryId = table.Column<int>(type: "int", nullable: true),
                    ForumId = table.Column<int>(type: "int", nullable: true),
                    LawyerDataId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseProfile_CaseCategory_CaseCategoryId",
                        column: x => x.CaseCategoryId,
                        principalTable: "CaseCategory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CaseProfile_CaseType_CaseTypeId",
                        column: x => x.CaseTypeId,
                        principalTable: "CaseType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CaseProfile_Forum_ForumId",
                        column: x => x.ForumId,
                        principalTable: "Forum",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CaseProfile_LawyerData_LawyerDataId",
                        column: x => x.LawyerDataId,
                        principalTable: "LawyerData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CaseProfile_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CaseProfileAppeals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseProfileId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseProfileAppeals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseProfileAppeals_CaseProfile_CaseProfileId",
                        column: x => x.CaseProfileId,
                        principalTable: "CaseProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CaseProfileAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseProfileId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseProfileAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseProfileAttachments_CaseProfile_CaseProfileId",
                        column: x => x.CaseProfileId,
                        principalTable: "CaseProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CaseProfileCaseHearings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Proceeding = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remind = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseProfileId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseProfileCaseHearings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseProfileCaseHearings_CaseProfile_CaseProfileId",
                        column: x => x.CaseProfileId,
                        principalTable: "CaseProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CaseProfileNotices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseProfileId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseProfileNotices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseProfileNotices_CaseProfile_CaseProfileId",
                        column: x => x.CaseProfileId,
                        principalTable: "CaseProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CaseProfileParties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseProfileId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseProfileParties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseProfileParties_CaseProfile_CaseProfileId",
                        column: x => x.CaseProfileId,
                        principalTable: "CaseProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseProfile_CaseCategoryId",
                table: "CaseProfile",
                column: "CaseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseProfile_CaseTypeId",
                table: "CaseProfile",
                column: "CaseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseProfile_ForumId",
                table: "CaseProfile",
                column: "ForumId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseProfile_LawyerDataId",
                table: "CaseProfile",
                column: "LawyerDataId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseProfile_StockCreationId",
                table: "CaseProfile",
                column: "StockCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseProfileAppeals_CaseProfileId",
                table: "CaseProfileAppeals",
                column: "CaseProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseProfileAttachments_CaseProfileId",
                table: "CaseProfileAttachments",
                column: "CaseProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseProfileCaseHearings_CaseProfileId",
                table: "CaseProfileCaseHearings",
                column: "CaseProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseProfileNotices_CaseProfileId",
                table: "CaseProfileNotices",
                column: "CaseProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseProfileParties_CaseProfileId",
                table: "CaseProfileParties",
                column: "CaseProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseProfileAppeals");

            migrationBuilder.DropTable(
                name: "CaseProfileAttachments");

            migrationBuilder.DropTable(
                name: "CaseProfileCaseHearings");

            migrationBuilder.DropTable(
                name: "CaseProfileNotices");

            migrationBuilder.DropTable(
                name: "CaseProfileParties");

            migrationBuilder.DropTable(
                name: "CaseProfile");
        }
    }
}
