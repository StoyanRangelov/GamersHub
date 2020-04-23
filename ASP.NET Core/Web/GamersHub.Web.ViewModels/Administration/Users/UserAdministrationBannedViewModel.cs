namespace GamersHub.Web.ViewModels.Administration.Users
{
    using System.Collections.Generic;

    public class UserAdministrationBannedViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<UserBannedViewModel> BannedUsers { get; set; }
    }
}
