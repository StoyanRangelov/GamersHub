namespace GamersHub.Services.Data.Tests.TestModels
{
    using System;

    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class TestPost : IMapFrom<Post>
    {
        public string CategoryName { get; set; }

        public int ForumId { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
