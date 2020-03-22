using Microsoft.EntityFrameworkCore.Migrations;

namespace GamersHub.Data.Migrations
{
    public partial class ChangedTopicColumnToNameInPostEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Topic",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Posts",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
