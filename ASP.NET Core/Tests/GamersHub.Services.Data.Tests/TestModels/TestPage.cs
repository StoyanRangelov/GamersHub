namespace GamersHub.Services.Data.Tests.TestModels
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class TestPage : IMapFrom<Page>
    {
        public string Name { get; set; }

        public string Content { get; set; }
    }
}
