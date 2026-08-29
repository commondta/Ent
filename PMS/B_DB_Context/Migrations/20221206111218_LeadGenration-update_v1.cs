using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class LeadGenrationupdate_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeadGenrationId",
                table: "LGSocialStatus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeadGenrationId",
                table: "LGActivities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LGSocialStatus_LeadGenrationId",
                table: "LGSocialStatus",
                column: "LeadGenrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LGActivities_LeadGenrationId",
                table: "LGActivities",
                column: "LeadGenrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_LGActivities_LeadGenration_LeadGenrationId",
                table: "LGActivities",
                column: "LeadGenrationId",
                principalTable: "LeadGenration",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LGSocialStatus_LeadGenration_LeadGenrationId",
                table: "LGSocialStatus",
                column: "LeadGenrationId",
                principalTable: "LeadGenration",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LGActivities_LeadGenration_LeadGenrationId",
                table: "LGActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_LGSocialStatus_LeadGenration_LeadGenrationId",
                table: "LGSocialStatus");

            migrationBuilder.DropIndex(
                name: "IX_LGSocialStatus_LeadGenrationId",
                table: "LGSocialStatus");

            migrationBuilder.DropIndex(
                name: "IX_LGActivities_LeadGenrationId",
                table: "LGActivities");

            migrationBuilder.DropColumn(
                name: "LeadGenrationId",
                table: "LGSocialStatus");

            migrationBuilder.DropColumn(
                name: "LeadGenrationId",
                table: "LGActivities");
        }
    }
}
