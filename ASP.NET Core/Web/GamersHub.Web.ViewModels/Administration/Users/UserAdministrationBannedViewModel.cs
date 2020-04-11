using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Administration.Users
{
    public class UserAdministrationBannedViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<UserBannedViewModel> BannedUsers { get; set; }
    }
}