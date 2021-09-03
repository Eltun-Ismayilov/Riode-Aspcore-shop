using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Riode.WebUI.Migrations
{
    public partial class xxx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cagetories");

            migrationBuilder.CreateTable(
                name: "OneCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateByUserId = table.Column<int>(type: "int", nullable: true),
                    CreateData = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteByUserId = table.Column<int>(type: "int", nullable: true),
                    DeleteData = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneCategories", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OneCategories");

            migrationBuilder.CreateTable(
                name: "Cagetories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateByUserId = table.Column<int>(type: "int", nullable: true),
                    CreateData = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteByUserId = table.Column<int>(type: "int", nullable: true),
                    DeleteData = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cagetories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cagetories_Cagetories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Cagetories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cagetories_ParentId",
                table: "Cagetories",
                column: "ParentId");
        }
    }
}
