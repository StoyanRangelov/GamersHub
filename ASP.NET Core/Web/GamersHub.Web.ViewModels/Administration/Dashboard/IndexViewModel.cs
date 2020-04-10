using System.Collections.Generic;
using GamersHub.Web.ViewModels.Administration.Users;

namespace GamersHub.Web.ViewModels.Administration.Dashboard
{
    public class IndexViewModel
    {
        public IEnumerable<GameDashboardViewModel> Games { get; set; }

        public IEnumerable<ForumDashboardViewModel> Forums { get; set; }

        public IEnumerable<CategoryDashboardViewModel> Categories { get; set; }

        public IEnumerable<PostDashboardViewModel> Posts { get; set; }

        public IEnumerable<ForumUserDashboardViewModel> ForumUsers { get; set; }

        public IEnumerable<GameUserDashboardViewModel> GameUsers { get; set; }

        public IEnumerable<UserBannedDashboardViewModel> BannedUsers { get; set; }

        public IEnumerable<UserInRoleViewModel> Moderators { get; set; }

        public IEnumerable<UserInRoleViewModel> Administrators { get; set; }
    }
}