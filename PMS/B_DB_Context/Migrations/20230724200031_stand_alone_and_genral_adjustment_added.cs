using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class stand_alone_and_genral_adjustment_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenralAdjustments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsGenralAdjustmentRequested = table.Column<bool>(type: "bit", nullable: true),
                    IsGenralAdjustmentApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsGenralAdjustmentClosed = table.Column<bool>(type: "bit", nullable: true),
                    InvoiceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Block = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PossessionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConstrucationStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_GenralAdjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GenralAdjustments_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GenralAdjustments_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "StandAlones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsStandAloneRequested = table.Column<bool>(type: "bit", nullable: true),
                    IsStandAloneApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsStandAloneClosed = table.Column<bool>(type: "bit", nullable: true),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Block = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PossessionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConstrucationStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_StandAlones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StandAlones_MemberProfile_MemberProfileId",
                        column: x => x.MemberProfileId,
                        principalTable: "MemberProfile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StandAlones_StockCreations_StockCreationId",
                        column: x => x.StockCreationId,
                        principalTable: "StockCreations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "GenralAdjustmentCharges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: true),
                    Adjustment = table.Column<int>(type: "int", nullable: true),
                    NetAmount = table.Column<int>(type: "int", nullable: true),
                    SapAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GenralAdjustmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenralAdjustmentCharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GenralAdjustmentCharges_GenralAdjustments_GenralAdjustmentId",
                        column: x => x.GenralAdjustmentId,
                        principalTable: "GenralAdjustments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StandAloneCharges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SapAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StandAloneId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandAloneCharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StandAloneCharges_StandAlones_StandAloneId",
                        column: x => x.StandAloneId,
                        principalTable: "StandAlones",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenralAdjustmentCharges_GenralAdjustmentId",
                table: "GenralAdjustmentCharges",
                column: "GenralAdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_GenralAdjustments_MemberProfileId",
                table: "GenralAdjustments",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_GenralAdjustments_StockCreationId",
                table: "GenralAdjustments",
                column: "StockCreationId");

            migrationBuilder.CreateIndex(
                name: "IX_StandAloneCharges_StandAloneId",
                table: "StandAloneCharges",
                column: "StandAloneId");

            migrationBuilder.CreateIndex(
                name: "IX_StandAlones_MemberProfileId",
                table: "StandAlones",
                column: "MemberProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_StandAlones_StockCreationId",
                table: "StandAlones",
                column: "StockCreationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenralAdjustmentCharges");

            migrationBuilder.DropTable(
                name: "StandAloneCharges");

            migrationBuilder.DropTable(
                name: "GenralAdjustments");

            migrationBuilder.DropTable(
                name: "StandAlones");
        }
    }
}
