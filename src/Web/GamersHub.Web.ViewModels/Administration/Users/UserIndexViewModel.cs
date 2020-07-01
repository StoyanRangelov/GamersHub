namespace GamersHub.Web.ViewModels.Administration.Users
{
    using System.Collections.Generic;

    public class UserIndexViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<UserViewModel> Users { get; set; }
    }
}
