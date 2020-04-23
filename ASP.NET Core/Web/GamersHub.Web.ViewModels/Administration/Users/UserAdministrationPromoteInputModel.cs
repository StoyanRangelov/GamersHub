namespace GamersHub.Web.ViewModels.Administration.Users
{
    public class UserAdministrationPromoteInputModel
    {
        public string UserId { get; set; }

        public UserPromoteViewModel User { get; set; }

        public string RoleName { get; set; }
    }
}
