using Microsoft.EntityFrameworkCore.Migrations;

namespace Riode.WebUI.Migrations
{
    public partial class adgava : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_blogCategories_blogCategories_ParentId",
                table: "blogCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_blogCategories",
                table: "blogCategories");

            migrationBuilder.RenameTable(
                name: "blogCategories",
                newName: "BlogCategories");

            migrationBuilder.RenameIndex(
                name: "IX_blogCategories_ParentId",
                table: "BlogCategories",
                newName: "IX_BlogCategories_ParentId");

            migrationBuilder.AddColumn<int>(
                name: "BlogCategoriesId",
                table: "Blogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogCategories",
                table: "BlogCategories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_BlogCategoriesId",
                table: "Blogs",
                column: "BlogCategoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogCategories_BlogCategories_ParentId",
                table: "BlogCategories",
                column: "ParentId",
                principalTable: "BlogCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_BlogCategories_BlogCategoriesId",
                table: "Blogs",
                column: "BlogCategoriesId",
                principalTable: "BlogCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogCategories_BlogCategories_ParentId",
                table: "BlogCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_BlogCategories_BlogCategoriesId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_BlogCategoriesId",
                table: "Blogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogCategories",
                table: "BlogCategories");

            migrationBuilder.DropColumn(
                name: "BlogCategoriesId",
                table: "Blogs");

            migrationBuilder.RenameTable(
                name: "BlogCategories",
                newName: "blogCategories");

            migrationBuilder.RenameIndex(
                name: "IX_BlogCategories_ParentId",
                table: "blogCategories",
                newName: "IX_blogCategories_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_blogCategories",
                table: "blogCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_blogCategories_blogCategories_ParentId",
                table: "blogCategories",
                column: "ParentId",
                principalTable: "blogCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
