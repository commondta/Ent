using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class memberProfile_update_v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_DealerProfile_DealerProfileId",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "LinkedId",
                table: "MemberProfile",
                newName: "LinkedInId");

            migrationBuilder.RenameColumn(
                name: "DealerProfileId",
                table: "Booking",
                newName: "DealerId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_DealerProfileId",
                table: "Booking",
                newName: "IX_Booking_DealerId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "NDCRequestForMemberCharges",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidityDate",
                table: "NDCRequestForMember",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PassportExpiryDate",
                table: "MemberProfile",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DOB",
                table: "MemberProfile",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CnicExpiryDate",
                table: "MemberProfile",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "NDCRequestForDealer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NDCRequestTypeID = table.Column<int>(type: "int", nullable: true),
                    TransferTypeID = table.Column<int>(type: "int", nullable: true),
                    MemberProfileId = table.Column<int>(type: "int", nullable: true),
                    DealerId = table.Column<int>(type: "int", nullable: true),
                    StockCreationId = table.Column<int>(type: "int", nullable: true),
                    Outstation = table.Column<bool>(type: "bit", nullable: false),
                    SlotTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NDCRequestForDealer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NDCRequestForDealer_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NDCRequestForDealer_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NDCRequestForDealer_NDCRequestType_NDCRequestTypeID",
                        column: x => x.NDCRequestTypeID,
                        principalTable: "NDCRequestType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_NDCRequestForDealer_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_NDCRequestForDealer_TransferType_TransferTypeID",
                        column: x => x.TransferTypeID,
                        principalTable: "TransferType",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "NDCRequestForDealerAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoucmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NDCRequestForDealerId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NDCRequestForDealerAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NDCRequestForDealerAttachments_NDCRequestForDealer_NDCRequestForDealerId",
                        column: x => x.NDCRequestForDealerId,
                        principalTable: "NDCRequestForDealer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NDCRequestForDealerCharges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NDCRequestForDealerId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NDCRequestForDealerCharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NDCRequestForDealerCharges_NDCRequestForDealer_NDCRequestForDealerId",
                        column: x => x.NDCRequestForDealerId,
                        principalTable: "NDCRequestForDealer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NDCRequestForDealer_DealerId",
                table: "NDCRequestForDealer",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_NDCRequestForDealer_MemberProfileId",
                table: "NDCRequestForDealer",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_NDCRequestForDealer_NDCRequestTypeID",
                table: "NDCRequestForDealer",
                column: "NDCRequestTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_NDCRequestForDealer_StockCreationId",
                table: "NDCRequestForDealer",
                column: "StockCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_NDCRequestForDealer_TransferTypeID",
                table: "NDCRequestForDealer",
                column: "TransferTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_NDCRequestForDealerAttachments_NDCRequestForDealerId",
                table: "NDCRequestForDealerAttachments",
                column: "NDCRequestForDealerId");

            migrationBuilder.CreateIndex(
                name: "IX_NDCRequestForDealerCharges_NDCRequestForDealerId",
                table: "NDCRequestForDealerCharges",
                column: "NDCRequestForDealerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Dealers_DealerId",
                table: "Booking",
                column: "DealerId",
                principalTable: "Dealers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Dealers_DealerId",
                table: "Booking");

            migrationBuilder.DropTable(
                name: "NDCRequestForDealerAttachments");

            migrationBuilder.DropTable(
                name: "NDCRequestForDealerCharges");

            migrationBuilder.DropTable(
                name: "NDCRequestForDealer");

            migrationBuilder.DropColumn(
                name: "ValidityDate",
                table: "NDCRequestForMember");

            migrationBuilder.RenameColumn(
                name: "LinkedInId",
                table: "MemberProfile",
                newName: "LinkedId");

            migrationBuilder.RenameColumn(
                name: "DealerId",
                table: "Booking",
                newName: "DealerProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_DealerId",
                table: "Booking",
                newName: "IX_Booking_DealerProfileId");

            migrationBuilder.AlterColumn<string>(
                name: "Amount",
                table: "NDCRequestForMemberCharges",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "PassportExpiryDate",
                table: "MemberProfile",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "DOB",
                table: "MemberProfile",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CnicExpiryDate",
                table: "MemberProfile",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_DealerProfile_DealerProfileId",
                table: "Booking",
                column: "DealerProfileId",
                principalTable: "DealerProfile",
                principalColumn: "Id");
        }
    }
}
