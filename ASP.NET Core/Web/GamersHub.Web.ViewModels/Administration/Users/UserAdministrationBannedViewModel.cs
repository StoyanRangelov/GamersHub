using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Administration.Users
{
    public class UserAdministrationBannedViewModel
    {
        public int PagesCount { get; set; }
        
        public int CurrentPage { get; set; }
        public IEnumerable<UserBannedViewModel> BannedUsers { get; set; }
    }
}