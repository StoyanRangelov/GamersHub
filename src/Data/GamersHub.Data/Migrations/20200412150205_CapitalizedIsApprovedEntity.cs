namespace GamersHub.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class CapitalizedIsApprovedEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isApproved",
                table: "PartyApplicants",
                newName: "IsApproved");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsApproved",
                table: "PartyApplicants",
                newName: "isApproved");
        }
    }
}
