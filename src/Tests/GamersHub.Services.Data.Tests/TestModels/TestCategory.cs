namespace GamersHub.Services.Data.Tests.TestModels
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class TestCategory : IMapFrom<Category>
    {
        public string Name { get; set; }
    }
}
