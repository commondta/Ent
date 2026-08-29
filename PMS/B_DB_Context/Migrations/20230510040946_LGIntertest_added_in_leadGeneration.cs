using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class LGIntertest_added_in_leadGeneration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LGInterests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyNature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeadGenrationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LGInterests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LGInterests_LeadGenration_LeadGenrationId",
                        column: x => x.LeadGenrationId,
                        principalTable: "LeadGenration",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LGInterests_LeadGenrationId",
                table: "LGInterests",
                column: "LeadGenrationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LGInterests");
        }
    }
}
