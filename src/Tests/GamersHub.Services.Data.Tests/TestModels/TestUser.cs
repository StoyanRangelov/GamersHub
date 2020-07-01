namespace GamersHub.Services.Data.Tests.TestModels
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class TestUser : IMapFrom<ApplicationUser>
    {
        public string Username { get; set; }
    }
}
