using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class govtTaxes_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransferReceiptProcessing_MemberProfile_MemberProfileId",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropIndex(
                name: "IX_TransferReceiptProcessing_MemberProfileId",
                table: "TransferReceiptProcessing");

            migrationBuilder.RenameColumn(
                name: "MemberProfileId",
                table: "TransferReceiptProcessing",
                newName: "SellerId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TransferDate",
                table: "TransferReceiptProcessing",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "TransferReceiptProcessing",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "FBRTAX236C",
                table: "TransferReceiptProcessing",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGovtProcessingTaxApproved",
                table: "TransferReceiptProcessing",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGovtProcessingTaxRequested",
                table: "TransferReceiptProcessing",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RegistryVerification",
                table: "TransferReceiptProcessing",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGovtTaxApproved",
                table: "NDC1",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGovtTaxRequested",
                table: "NDC1",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FBR236C",
                table: "GlobalChargeSetup",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RegistryVerification",
                table: "GlobalChargeSetup",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GovtBuyerCharges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SapAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransferReceiptProcessingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovtBuyerCharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GovtBuyerCharges_TransferReceiptProcessing_TransferReceiptProcessingId",
                        column: x => x.TransferReceiptProcessingId,
                        principalTable: "TransferReceiptProcessing",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GovtSellerCharges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SapAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransferReceiptProcessingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GovtSellerCharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GovtSellerCharges_TransferReceiptProcessing_TransferReceiptProcessingId",
                        column: x => x.TransferReceiptProcessingId,
                        principalTable: "TransferReceiptProcessing",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GovtBuyerCharges_TransferReceiptProcessingId",
                table: "GovtBuyerCharges",
                column: "TransferReceiptProcessingId");

            migrationBuilder.CreateIndex(
                name: "IX_GovtSellerCharges_TransferReceiptProcessingId",
                table: "GovtSellerCharges",
                column: "TransferReceiptProcessingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GovtBuyerCharges");

            migrationBuilder.DropTable(
                name: "GovtSellerCharges");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "FBRTAX236C",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "IsGovtProcessingTaxApproved",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "IsGovtProcessingTaxRequested",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "RegistryVerification",
                table: "TransferReceiptProcessing");

            migrationBuilder.DropColumn(
                name: "IsGovtTaxApproved",
                table: "NDC1");

            migrationBuilder.DropColumn(
                name: "IsGovtTaxRequested",
                table: "NDC1");

            migrationBuilder.DropColumn(
                name: "FBR236C",
                table: "GlobalChargeSetup");

            migrationBuilder.DropColumn(
                name: "RegistryVerification",
                table: "GlobalChargeSetup");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "TransferReceiptProcessing",
                newName: "MemberProfileId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TransferDate",
                table: "TransferReceiptProcessing",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransferReceiptProcessing_MemberProfileId",
                table: "TransferReceiptProcessing",
                column: "MemberProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransferReceiptProcessing_MemberProfile_MemberProfileId",
                table: "TransferReceiptProcessing",
                column: "MemberProfileId",
                principalTable: "MemberProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
