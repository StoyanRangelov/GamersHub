namespace GamersHub.Services.Data.Tests.TestModels
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class TestPartyApplicant : IMapFrom<PartyApplicant>
    {
        public string ApplicantUsername { get; set; }

        public string PartyGame { get; set; }
    }
}
