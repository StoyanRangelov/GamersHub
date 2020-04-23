namespace GamersHub.Services.Data.Tests.TestModels
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class TestReply : IMapFrom<Reply>
    {
        public int PostId { get; set; }

        public string Content { get; set; }
    }
}
