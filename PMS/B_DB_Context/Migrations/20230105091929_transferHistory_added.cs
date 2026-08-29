using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class transferHistory_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransferHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransferFromImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransferToImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CombineImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsHold = table.Column<bool>(type: "bit", nullable: true),
                    StockCreationId = table.Column<int>(type: "int", nullable: true),
                    OwneryProfileId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_TransferHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferHistory_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransferHistory_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "TansferHistoryJointMember",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Relationship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransferHistoryId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TansferHistoryJointMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TansferHistoryJointMember_TransferHistory_TransferHistoryId",
                        column: x => x.TransferHistoryId,
                        principalTable: "TransferHistory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TansferHistoryNominee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Relationship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransferHistoryId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TansferHistoryNominee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TansferHistoryNominee_TransferHistory_TransferHistoryId",
                        column: x => x.TransferHistoryId,
                        principalTable: "TransferHistory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TansferHistoryJointMember_TransferHistoryId",
                table: "TansferHistoryJointMember",
                column: "TransferHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TansferHistoryNominee_TransferHistoryId",
                table: "TansferHistoryNominee",
                column: "TransferHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferHistory_MemberProfileId",
                table: "TransferHistory",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferHistory_StockCreationId",
                table: "TransferHistory",
                column: "StockCreationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TansferHistoryJointMember");

            migrationBuilder.DropTable(
                name: "TansferHistoryNominee");

            migrationBuilder.DropTable(
                name: "TransferHistory");
        }
    }
}
