namespace GamersHub.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RenamedCapacityToSizeInPartyEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Parties");

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Parties",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Parties");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Parties",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
