using Microsoft.EntityFrameworkCore.Migrations;

namespace Riode.WebUI.Migrations
{
    public partial class Onecategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OneCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OneCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "OneCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OneCategories_ParentId",
                table: "OneCategories",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_OneCategories_OneCategories_ParentId",
                table: "OneCategories",
                column: "ParentId",
                principalTable: "OneCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OneCategories_OneCategories_ParentId",
                table: "OneCategories");

            migrationBuilder.DropIndex(
                name: "IX_OneCategories_ParentId",
                table: "OneCategories");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "OneCategories");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OneCategories");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "OneCategories");
        }
    }
}
