using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class NDCRequestForMember_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NDCRequestForMember",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NDCRequestTypeID = table.Column<int>(type: "int", nullable: true),
                    TransferTypeID = table.Column<int>(type: "int", nullable: true),
                    MemberProfileId = table.Column<int>(type: "int", nullable: true),
                    StockCreationId = table.Column<int>(type: "int", nullable: true),
                    Outstation = table.Column<bool>(type: "bit", nullable: false),
                    SlotTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NDCRequestForMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NDCRequestForMember_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NDCRequestForMember_NDCRequestType_NDCRequestTypeID",
                        column: x => x.NDCRequestTypeID,
                        principalTable: "NDCRequestType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_NDCRequestForMember_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_NDCRequestForMember_TransferType_TransferTypeID",
                        column: x => x.TransferTypeID,
                        principalTable: "TransferType",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "NDCRequestForMemberAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoucmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NDCRequestForMemberId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NDCRequestForMemberAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NDCRequestForMemberAttachments_NDCRequestForMember_NDCRequestForMemberId",
                        column: x => x.NDCRequestForMemberId,
                        principalTable: "NDCRequestForMember",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NDCRequestForMemberCharges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NDCRequestForMemberId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NDCRequestForMemberCharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NDCRequestForMemberCharges_NDCRequestForMember_NDCRequestForMemberId",
                        column: x => x.NDCRequestForMemberId,
                        principalTable: "NDCRequestForMember",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NDCRequestForMember_MemberProfileId",
                table: "NDCRequestForMember",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_NDCRequestForMember_NDCRequestTypeID",
                table: "NDCRequestForMember",
                column: "NDCRequestTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_NDCRequestForMember_StockCreationId",
                table: "NDCRequestForMember",
                column: "StockCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_NDCRequestForMember_TransferTypeID",
                table: "NDCRequestForMember",
                column: "TransferTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_NDCRequestForMemberAttachments_NDCRequestForMemberId",
                table: "NDCRequestForMemberAttachments",
                column: "NDCRequestForMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_NDCRequestForMemberCharges_NDCRequestForMemberId",
                table: "NDCRequestForMemberCharges",
                column: "NDCRequestForMemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NDCRequestForMemberAttachments");

            migrationBuilder.DropTable(
                name: "NDCRequestForMemberCharges");

            migrationBuilder.DropTable(
                name: "NDCRequestForMember");
        }
    }
}
