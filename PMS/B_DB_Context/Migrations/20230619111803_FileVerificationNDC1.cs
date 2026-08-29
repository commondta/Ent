using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class FileVerificationNDC1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileVerificationNDC1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockCreationId = table.Column<int>(type: "int", nullable: true),
                    MemberProfileId = table.Column<int>(type: "int", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileVerificationNDC1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileVerificationNDC1_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FileVerificationNDC1_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "FileVerificationNDC1Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoucmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileVerificationNDC1Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileVerificationNDC1Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileVerificationNDC1Attachments_FileVerificationNDC1_FileVerificationNDC1Id",
                        column: x => x.FileVerificationNDC1Id,
                        principalTable: "FileVerificationNDC1",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FileVerificationNDC1CheckList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlertNarration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileVerificationNDC1Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileVerificationNDC1CheckList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileVerificationNDC1CheckList_FileVerificationNDC1_FileVerificationNDC1Id",
                        column: x => x.FileVerificationNDC1Id,
                        principalTable: "FileVerificationNDC1",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FileVerificationNDC1PowerOfAttorey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cnic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileVerificationNDC1Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileVerificationNDC1PowerOfAttorey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileVerificationNDC1PowerOfAttorey_FileVerificationNDC1_FileVerificationNDC1Id",
                        column: x => x.FileVerificationNDC1Id,
                        principalTable: "FileVerificationNDC1",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileVerificationNDC1_MemberProfileId",
                table: "FileVerificationNDC1",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_FileVerificationNDC1_StockCreationId",
                table: "FileVerificationNDC1",
                column: "StockCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_FileVerificationNDC1Attachments_FileVerificationNDC1Id",
                table: "FileVerificationNDC1Attachments",
                column: "FileVerificationNDC1Id");

            migrationBuilder.CreateIndex(
                name: "IX_FileVerificationNDC1CheckList_FileVerificationNDC1Id",
                table: "FileVerificationNDC1CheckList",
                column: "FileVerificationNDC1Id");

            migrationBuilder.CreateIndex(
                name: "IX_FileVerificationNDC1PowerOfAttorey_FileVerificationNDC1Id",
                table: "FileVerificationNDC1PowerOfAttorey",
                column: "FileVerificationNDC1Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileVerificationNDC1Attachments");

            migrationBuilder.DropTable(
                name: "FileVerificationNDC1CheckList");

            migrationBuilder.DropTable(
                name: "FileVerificationNDC1PowerOfAttorey");

            migrationBuilder.DropTable(
                name: "FileVerificationNDC1");
        }
    }
}
