using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class TransferSetReceiving_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransferSetReceivings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsSitePlanRequested = table.Column<bool>(type: "bit", nullable: true),
                    IsSitePlanApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsRequestClosed = table.Column<bool>(type: "bit", nullable: true),
                    Block = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PossessionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConstrucationStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlotDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SlotHour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlotMintues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NDCRequestType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransferType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealerCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstateName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Depositor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberProfileId = table.Column<int>(type: "int", nullable: true),
                    StockCreationId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_TransferSetReceivings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferSetReceivings_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransferSetReceivings_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "TransferSetReceivingAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoucmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransferSetReceivingId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_TransferSetReceivingAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferSetReceivingAttachments_TransferSetReceivings_TransferSetReceivingId",
                        column: x => x.TransferSetReceivingId,
                        principalTable: "TransferSetReceivings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferSetReceivingAttachments_TransferSetReceivingId",
                table: "TransferSetReceivingAttachments",
                column: "TransferSetReceivingId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferSetReceivings_MemberProfileId",
                table: "TransferSetReceivings",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferSetReceivings_StockCreationId",
                table: "TransferSetReceivings",
                column: "StockCreationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferSetReceivingAttachments");

            migrationBuilder.DropTable(
                name: "TransferSetReceivings");
        }
    }
}
