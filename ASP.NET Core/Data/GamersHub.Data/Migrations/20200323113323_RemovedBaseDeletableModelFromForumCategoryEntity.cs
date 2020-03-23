using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GamersHub.Data.Migrations
{
    public partial class RemovedBaseDeletableModelFromForumCategoryEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ForumCategories_IsDeleted",
                table: "ForumCategories");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ForumCategories");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ForumCategories");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ForumCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ForumCategories");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "ForumCategories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ForumCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "ForumCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ForumCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ForumCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "ForumCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ForumCategories_IsDeleted",
                table: "ForumCategories",
                column: "IsDeleted");
        }
    }
}
