namespace GamersHub.Services.Data.Tests.TestModels
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class TestReview : IMapFrom<Review>
    {
        public int GameId { get; set; }

        public bool IsPositive { get; set; }

        public string Content { get; set; }
    }
}
