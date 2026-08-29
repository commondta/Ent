using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class globalchargessetup_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GlobalChargeGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeGroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalChargeGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChargeGroupType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: true),
                    ChargeTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GlobalChargeGroupId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargeGroupType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChargeGroupType_GlobalChargeGroup_GlobalChargeGroupId",
                        column: x => x.GlobalChargeGroupId,
                        principalTable: "GlobalChargeGroup",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GlobalChargeSetup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConstructionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PossessionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayableTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GlobalChargeGroupId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    PhaseId = table.Column<int>(type: "int", nullable: true),
                    BlockId = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    PropertyTypeId = table.Column<int>(type: "int", nullable: true),
                    NatureId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalChargeSetup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GlobalChargeSetup_Blocks_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Blocks",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_GlobalChargeSetup_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_GlobalChargeSetup_GlobalChargeGroup_GlobalChargeGroupId",
                        column: x => x.GlobalChargeGroupId,
                        principalTable: "GlobalChargeGroup",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GlobalChargeSetup_Natures_NatureId",
                        column: x => x.NatureId,
                        principalTable: "Natures",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_GlobalChargeSetup_Phases_PhaseId",
                        column: x => x.PhaseId,
                        principalTable: "Phases",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_GlobalChargeSetup_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_GlobalChargeSetup_PropertyTypes_PropertyTypeId",
                        column: x => x.PropertyTypeId,
                        principalTable: "PropertyTypes",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "GlobalChargeDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: true),
                    ChargeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    MultiplyBySize = table.Column<bool>(type: "bit", nullable: true),
                    GlobalChargeSetupId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalChargeDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GlobalChargeDetail_GlobalChargeSetup_GlobalChargeSetupId",
                        column: x => x.GlobalChargeSetupId,
                        principalTable: "GlobalChargeSetup",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChargeGroupType_GlobalChargeGroupId",
                table: "ChargeGroupType",
                column: "GlobalChargeGroupId",
                unique: true,
                filter: "[GlobalChargeGroupId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeDetail_GlobalChargeSetupId",
                table: "GlobalChargeDetail",
                column: "GlobalChargeSetupId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_BlockId",
                table: "GlobalChargeSetup",
                column: "BlockId",
                unique: true,
                filter: "[BlockId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_CategoryId",
                table: "GlobalChargeSetup",
                column: "CategoryId",
                unique: true,
                filter: "[CategoryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_GlobalChargeGroupId",
                table: "GlobalChargeSetup",
                column: "GlobalChargeGroupId",
                unique: true,
                filter: "[GlobalChargeGroupId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_NatureId",
                table: "GlobalChargeSetup",
                column: "NatureId",
                unique: true,
                filter: "[NatureId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_PhaseId",
                table: "GlobalChargeSetup",
                column: "PhaseId",
                unique: true,
                filter: "[PhaseId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_ProjectId",
                table: "GlobalChargeSetup",
                column: "ProjectId",
                unique: true,
                filter: "[ProjectId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalChargeSetup_PropertyTypeId",
                table: "GlobalChargeSetup",
                column: "PropertyTypeId",
                unique: true,
                filter: "[PropertyTypeId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChargeGroupType");

            migrationBuilder.DropTable(
                name: "GlobalChargeDetail");

            migrationBuilder.DropTable(
                name: "GlobalChargeSetup");

            migrationBuilder.DropTable(
                name: "GlobalChargeGroup");
        }
    }
}
