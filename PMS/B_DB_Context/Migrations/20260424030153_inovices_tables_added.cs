using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class inovices_tables_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inovices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeTypeId = table.Column<int>(type: "int", nullable: true),
                    ChargeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormId = table.Column<int>(type: "int", nullable: true),
                    SapAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceGroup = table.Column<short>(type: "smallint", nullable: false),
                    PostingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    AmountDue = table.Column<double>(type: "float", nullable: true),
                    AmountRecieved = table.Column<double>(type: "float", nullable: true),
                    DocTotal = table.Column<double>(type: "float", nullable: true),
                    InvoiceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReciptNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsInProress = table.Column<bool>(type: "bit", nullable: true),
                    IsSapPosting = table.Column<bool>(type: "bit", nullable: true),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstallementNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Days = table.Column<int>(type: "int", nullable: true),
                    PaymentTypeId = table.Column<int>(type: "int", nullable: true),
                    ReferenceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockCreationId = table.Column<int>(type: "int", nullable: true),
                    MemberProfileId = table.Column<int>(type: "int", nullable: true),
                    DealerId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Inovices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inovices_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inovices_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inovices_StockCreation_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreation",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inovices_DealerId",
                table: "Inovices",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_Inovices_MemberProfileId",
                table: "Inovices",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Inovices_StockCreationId",
                table: "Inovices",
                column: "StockCreationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inovices");
        }
    }
}
