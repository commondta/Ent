using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class registrationNoProfile_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "PlanInformation");

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "PaymentPlanSetup",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "MemberProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "MemberProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                name: "RegistrationNoProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrespondenceAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrespondenceEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrespondenceMobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrespondenceWhatsappNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockCreationId = table.Column<int>(type: "int", nullable: false),
                    MemberProfileId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationNoProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationNoProfile_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrationNoProfile_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlertName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlertNarration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationNoProfileId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alerts_RegistrationNoProfile_RegistrationNoProfileId",
                        column: x => x.RegistrationNoProfileId,
                        principalTable: "RegistrationNoProfile",
                        principalColumn: "Id");
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
                    RegistrationNoProfileId = table.Column<int>(type: "int", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_Attachments_RegistrationNoProfile_RegistrationNoProfileId",
                        column: x => x.RegistrationNoProfileId,
                        principalTable: "RegistrationNoProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SoftLock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationNoProfileId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftLock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoftLock_RegistrationNoProfile_RegistrationNoProfileId",
                        column: x => x.RegistrationNoProfileId,
                        principalTable: "RegistrationNoProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_RegistrationNoProfileId",
                table: "Alerts",
                column: "RegistrationNoProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_DealerProfileId",
                table: "Attachments",
                column: "DealerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_RegistrationNoProfileId",
                table: "Attachments",
                column: "RegistrationNoProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_EstateDetail_DealerProfileId",
                table: "EstateDetail",
                column: "DealerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationNoProfile_MemberProfileId",
                table: "RegistrationNoProfile",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationNoProfile_StockCreationId",
                table: "RegistrationNoProfile",
                column: "StockCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftLock_RegistrationNoProfileId",
                table: "SoftLock",
                column: "RegistrationNoProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "EstateDetail");

            migrationBuilder.DropTable(
                name: "SoftLock");

            migrationBuilder.DropTable(
                name: "DealerProfile");

            migrationBuilder.DropTable(
                name: "RegistrationNoProfile");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "PaymentPlanSetup");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "MemberProfile");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "MemberProfile");

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "PlanInformation",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
