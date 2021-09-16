using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Riode.WebUI.Migrations
{
    public partial class BlogPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "DataTime",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Blogs");

            migrationBuilder.RenameColumn(
                name: "PostBody2",
                table: "Blogs",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "PostBody1",
                table: "Blogs",
                newName: "ImagePati");

            migrationBuilder.RenameColumn(
                name: "PostAuthor",
                table: "Blogs",
                newName: "Body");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedDate",
                table: "Blogs",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishedDate",
                table: "Blogs");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Blogs",
                newName: "PostBody2");

            migrationBuilder.RenameColumn(
                name: "ImagePati",
                table: "Blogs",
                newName: "PostBody1");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Blogs",
                newName: "PostAuthor");

            migrationBuilder.AddColumn<int>(
                name: "Comments",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DataTime",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
