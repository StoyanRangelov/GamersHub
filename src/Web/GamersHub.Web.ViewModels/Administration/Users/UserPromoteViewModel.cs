namespace GamersHub.Web.ViewModels.Administration.Users
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class UserPromoteViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }
    }
}
