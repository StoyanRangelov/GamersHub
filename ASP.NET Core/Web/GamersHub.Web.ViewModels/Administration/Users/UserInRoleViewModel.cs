namespace GamersHub.Web.ViewModels.Administration.Users
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class UserInRoleViewModel : IMapFrom<ApplicationUser>
    {
        public string Username { get; set; }
    }
}
