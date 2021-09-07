using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Riode.WebUI.Migrations
{
    public partial class ContactPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contects");

            migrationBuilder.CreateTable(
                name: "ContactPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerdData = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AnswerByUserId = table.Column<int>(type: "int", nullable: true),
                    CreateByUserId = table.Column<int>(type: "int", nullable: true),
                    CreateData = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteByUserId = table.Column<int>(type: "int", nullable: true),
                    DeleteData = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactPosts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactPosts");

            migrationBuilder.CreateTable(
                name: "contects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contects", x => x.Id);
                });
        }
    }
}
