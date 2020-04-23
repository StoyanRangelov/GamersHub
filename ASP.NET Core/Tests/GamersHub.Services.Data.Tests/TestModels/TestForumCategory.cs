namespace GamersHub.Services.Data.Tests.TestModels
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class TestForumCategory : IMapFrom<ForumCategory>
    {
        public string CategoryName { get; set; }

        public int ForumId { get; set; }
    }
}
