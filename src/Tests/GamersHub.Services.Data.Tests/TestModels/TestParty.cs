namespace GamersHub.Services.Data.Tests.TestModels
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class TestParty : IMapFrom<Party>
    {
        public string CreatorUsername { get; set; }

        public string Game { get; set; }
    }
}
