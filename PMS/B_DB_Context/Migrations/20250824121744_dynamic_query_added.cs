using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B_DB_Context.Migrations
{
    public partial class dynamic_query_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DynamicQueries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessUsers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrintTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SqlQuery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicQueries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QueryParams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    DynamicQueryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryParams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueryParams_DynamicQueries_DynamicQueryId",
                        column: x => x.DynamicQueryId,
                        principalTable: "DynamicQueries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QueryParamOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParamId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QueryParamId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryParamOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueryParamOptions_QueryParams_QueryParamId",
                        column: x => x.QueryParamId,
                        principalTable: "QueryParams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QueryParamOptions_QueryParamId",
                table: "QueryParamOptions",
                column: "QueryParamId");

            migrationBuilder.CreateIndex(
                name: "IX_QueryParams_DynamicQueryId",
                table: "QueryParams",
                column: "DynamicQueryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QueryParamOptions");

            migrationBuilder.DropTable(
                name: "QueryParams");

            migrationBuilder.DropTable(
                name: "DynamicQueries");
        }
    }
}
