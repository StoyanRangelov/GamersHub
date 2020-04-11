using System.Collections.Generic;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Users
{
    public class UserIndexViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<UserViewModel> Users { get; set; }
    }
}