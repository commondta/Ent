using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class Booking_update_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingNominee_Bookings_BookingId",
                table: "BookingNominee");

            migrationBuilder.DropTable(
                name: "BookingDetail");

            migrationBuilder.DropTable(
                name: "BookingPersonalInfoDetail");

            migrationBuilder.DropTable(
                name: "BookingPropertyRequiredDetail");

            migrationBuilder.DropTable(
                name: "BookingScheduleBodyPaymentPlanDetail");

            migrationBuilder.DropTable(
                name: "BookingScheduleHeaderDetail");

            migrationBuilder.DropTable(
                name: "BookingSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "CNIC",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "Created_at",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "MobileNo",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "Relationship",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "Updated_at",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "CNIC",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CNICExpiryDate",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Created_at",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DOB",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DealerCode",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DealerName",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DocumentDate",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "GuardianName",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "MemberCode",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "MemberName",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "MemberStatus",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "NICOP",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PassportExpiryDate",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Updated_at",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "Bookings");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "Booking");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Booking",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SourceOfInfo",
                table: "Booking",
                newName: "Remarks");

            migrationBuilder.RenameColumn(
                name: "RelationWithGuardian",
                table: "Booking",
                newName: "CorrespondenceWhatsappNo");

            migrationBuilder.RenameColumn(
                name: "Pic",
                table: "Booking",
                newName: "CorrespondenceMobileNo");

            migrationBuilder.RenameColumn(
                name: "Passport",
                table: "Booking",
                newName: "CorrespondenceEmail");

            migrationBuilder.RenameColumn(
                name: "POC",
                table: "Booking",
                newName: "CorrespondenceAddress");

            migrationBuilder.RenameColumn(
                name: "Overseas",
                table: "Booking",
                newName: "BookingConfirmationReceiptNo");

            migrationBuilder.RenameColumn(
                name: "Nationality",
                table: "Booking",
                newName: "AdvanceReceiptNo");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "BookingNominee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "BookingNominee",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "BookingNominee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BookingNominee",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BookingNominee",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "BookingNominee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "MemberProfileId",
                table: "BookingNominee",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "BookingNominee",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AdvanceAmount",
                table: "Booking",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "AdvanceReceiptDate",
                table: "Booking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAmount",
                table: "Booking",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingConfirmationReceiptDate",
                table: "Booking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "BookingPrice",
                table: "Booking",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Booking",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Booking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DealerProfileId",
                table: "Booking",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Booking",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Booking",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "JointOwners",
                table: "Booking",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Booking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "MemberProfileId",
                table: "Booking",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "Booking",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockCreationId",
                table: "Booking",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BookingJointMember",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberProfileId = table.Column<int>(type: "int", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingJointMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingJointMember_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookingJointMember_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookingProcessingCharges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingProcessingCharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingProcessingCharges_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookingSchedulePaymentPlanDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstallementNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Days = table.Column<int>(type: "int", nullable: false),
                    InstallementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReciptNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingSchedulePaymentPlanDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingSchedulePaymentPlanDetail_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingNominee_MemberProfileId",
                table: "BookingNominee",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_DealerProfileId",
                table: "Booking",
                column: "DealerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_MemberProfileId",
                table: "Booking",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_StockCreationId",
                table: "Booking",
                column: "StockCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingJointMember_BookingId",
                table: "BookingJointMember",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingJointMember_MemberProfileId",
                table: "BookingJointMember",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingProcessingCharges_BookingId",
                table: "BookingProcessingCharges",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingSchedulePaymentPlanDetail_BookingId",
                table: "BookingSchedulePaymentPlanDetail",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_DealerProfile_DealerProfileId",
                table: "Booking",
                column: "DealerProfileId",
                principalTable: "DealerProfile",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_MemberProfile_MemberProfileId",
                table: "Booking",
                column: "MemberProfileId",
                principalTable: "MemberProfile",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_StockCreations_StockCreationId",
                table: "Booking",
                column: "StockCreationId",
                principalTable: "StockCreations",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingNominee_Booking_BookingId",
                table: "BookingNominee",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingNominee_MemberProfile_MemberProfileId",
                table: "BookingNominee",
                column: "MemberProfileId",
                principalTable: "MemberProfile",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_DealerProfile_DealerProfileId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_MemberProfile_MemberProfileId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_StockCreations_StockCreationId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingNominee_Booking_BookingId",
                table: "BookingNominee");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingNominee_MemberProfile_MemberProfileId",
                table: "BookingNominee");

            migrationBuilder.DropTable(
                name: "BookingJointMember");

            migrationBuilder.DropTable(
                name: "BookingProcessingCharges");

            migrationBuilder.DropTable(
                name: "BookingSchedulePaymentPlanDetail");

            migrationBuilder.DropIndex(
                name: "IX_BookingNominee_MemberProfileId",
                table: "BookingNominee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_DealerProfileId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_MemberProfileId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_StockCreationId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "MemberProfileId",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "BookingNominee");

            migrationBuilder.DropColumn(
                name: "AdvanceAmount",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "AdvanceReceiptDate",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "BalanceAmount",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "BookingConfirmationReceiptDate",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "BookingPrice",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "DealerProfileId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "JointOwners",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "MemberProfileId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "StockCreationId",
                table: "Booking");

            migrationBuilder.RenameTable(
                name: "Booking",
                newName: "Bookings");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Bookings",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Remarks",
                table: "Bookings",
                newName: "SourceOfInfo");

            migrationBuilder.RenameColumn(
                name: "CorrespondenceWhatsappNo",
                table: "Bookings",
                newName: "RelationWithGuardian");

            migrationBuilder.RenameColumn(
                name: "CorrespondenceMobileNo",
                table: "Bookings",
                newName: "Pic");

            migrationBuilder.RenameColumn(
                name: "CorrespondenceEmail",
                table: "Bookings",
                newName: "Passport");

            migrationBuilder.RenameColumn(
                name: "CorrespondenceAddress",
                table: "Bookings",
                newName: "POC");

            migrationBuilder.RenameColumn(
                name: "BookingConfirmationReceiptNo",
                table: "Bookings",
                newName: "Overseas");

            migrationBuilder.RenameColumn(
                name: "AdvanceReceiptNo",
                table: "Bookings",
                newName: "Nationality");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "BookingNominee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "BookingNominee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CNIC",
                table: "BookingNominee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_at",
                table: "BookingNominee",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobileNo",
                table: "BookingNominee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BookingNominee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Relationship",
                table: "BookingNominee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated_at",
                table: "BookingNominee",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "BookingNominee",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "BookingNominee",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CNIC",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CNICExpiryDate",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_at",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DOB",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DealerCode",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DealerName",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DocumentDate",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuardianName",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberCode",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberName",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberStatus",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NICOP",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PassportExpiryDate",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated_at",
                table: "Bookings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "Bookings",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "Bookings",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "BookingDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    BookingFee = table.Column<double>(type: "float", nullable: true),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LandPrice = table.Column<double>(type: "float", nullable: true),
                    PropertyNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SportsFund = table.Column<double>(type: "float", nullable: true),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingDetail_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingPersonalInfoDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    CorrespondenceAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrespondenceEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrespondenceMobileNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrespondenceWhatsappNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RepresentativeMobileNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepresentativeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPersonalInfoDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingPersonalInfoDetail_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingPropertyRequiredDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Floor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Project = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RealEstateType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPropertyRequiredDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingPropertyRequiredDetail_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingSchedule_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingScheduleBodyPaymentPlanDetail",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingScheduleId = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InstallementAmount = table.Column<double>(type: "float", nullable: true),
                    InstallementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InstallementDiscount = table.Column<double>(type: "float", nullable: true),
                    InstallementNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetAmount = table.Column<double>(type: "float", nullable: true),
                    Remarks = table.Column<double>(type: "float", nullable: true),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingScheduleBodyPaymentPlanDetail", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BookingScheduleBodyPaymentPlanDetail_BookingSchedule_BookingScheduleId",
                        column: x => x.BookingScheduleId,
                        principalTable: "BookingSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingScheduleHeaderDetail",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingScheduleId = table.Column<int>(type: "int", nullable: false),
                    AdvanceAmount = table.Column<double>(type: "float", nullable: true),
                    BalanceAmount = table.Column<double>(type: "float", nullable: true),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BookingPrice = table.Column<double>(type: "float", nullable: true),
                    Chalan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentPlanCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiptDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingScheduleHeaderDetail", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BookingScheduleHeaderDetail_BookingSchedule_BookingScheduleId",
                        column: x => x.BookingScheduleId,
                        principalTable: "BookingSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetail_BookingId",
                table: "BookingDetail",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingPersonalInfoDetail_BookingId",
                table: "BookingPersonalInfoDetail",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingPropertyRequiredDetail_BookingId",
                table: "BookingPropertyRequiredDetail",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingSchedule_BookingId",
                table: "BookingSchedule",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingScheduleBodyPaymentPlanDetail_BookingScheduleId",
                table: "BookingScheduleBodyPaymentPlanDetail",
                column: "BookingScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingScheduleHeaderDetail_BookingScheduleId",
                table: "BookingScheduleHeaderDetail",
                column: "BookingScheduleId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingNominee_Bookings_BookingId",
                table: "BookingNominee",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
