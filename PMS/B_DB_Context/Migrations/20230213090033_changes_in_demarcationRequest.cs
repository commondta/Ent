using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class changes_in_demarcationRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewDemarcationRequest_GlobalChargeSetup_GlobalChargeSetupId",
                table: "NewDemarcationRequest");

            migrationBuilder.DropIndex(
                name: "IX_NewDemarcationRequest_GlobalChargeSetupId",
                table: "NewDemarcationRequest");

            migrationBuilder.DropColumn(
                name: "GlobalChargeSetupId",
                table: "NewDemarcationRequest");

            migrationBuilder.CreateTable(
                name: "NewDemarcationRequestDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rate = table.Column<int>(type: "int", nullable: true),
                    NewDemarcationRequestId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewDemarcationRequestDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewDemarcationRequestDetail_NewDemarcationRequest_NewDemarcationRequestId",
                        column: x => x.NewDemarcationRequestId,
                        principalTable: "NewDemarcationRequest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewDemarcationRequestDetail_NewDemarcationRequestId",
                table: "NewDemarcationRequestDetail",
                column: "NewDemarcationRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewDemarcationRequestDetail");

            migrationBuilder.AddColumn<int>(
                name: "GlobalChargeSetupId",
                table: "NewDemarcationRequest",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewDemarcationRequest_GlobalChargeSetupId",
                table: "NewDemarcationRequest",
                column: "GlobalChargeSetupId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewDemarcationRequest_GlobalChargeSetup_GlobalChargeSetupId",
                table: "NewDemarcationRequest",
                column: "GlobalChargeSetupId",
                principalTable: "GlobalChargeSetup",
                principalColumn: "Id");
        }
    }
}
