namespace GamersHub.Services.Data.Tests.TestModels
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class TestForum : IMapFrom<Forum>
    {
        public string Name { get; set; }
    }
}
