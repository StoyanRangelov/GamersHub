using System.Collections.Generic;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using GamersHub.Web.ViewModels.Forums;

namespace GamersHub.Web.ViewModels.Administration.Forums
{
    public class ForumAdministrationViewModel : IMapFrom<Forum>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int PostsCount { get; set; }

        public int ForumCategoriesCount { get; set; }
    }
}