namespace GamersHub.Web.ViewModels.Administration.Users
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class RolePromoteViewModel : IMapFrom<ApplicationRole>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
