namespace GamersHub.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class FixedTypoInUserEntityPropertyGamingExperience : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GaminExperience",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "GamingExperience",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GamingExperience",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "GaminExperience",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
