namespace GamersHub.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RemovedIsApprovedAndIsDeclinedAndAddedApplicationStatusTypeToEntityPartyApplicant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "PartyApplicants");

            migrationBuilder.DropColumn(
                name: "IsDeclined",
                table: "PartyApplicants");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationStatus",
                table: "PartyApplicants",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationStatus",
                table: "PartyApplicants");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "PartyApplicants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeclined",
                table: "PartyApplicants",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
