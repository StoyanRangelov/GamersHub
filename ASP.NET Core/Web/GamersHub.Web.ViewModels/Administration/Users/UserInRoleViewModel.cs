using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Users
{
    public class UserInRoleViewModel : IMapFrom<ApplicationUser>
    {
        public string Username { get; set; }
    }
}