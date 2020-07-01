namespace GamersHub.Services.Data.Tests.TestModels
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class TestGame : IMapFrom<Game>
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }
    }
}
