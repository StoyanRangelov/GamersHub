using System;
using System.Collections.Generic;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Forums
{
    public class ForumDeleteViewModel : IMapFrom<Forum>
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public int PostsCount { get; set; }

        public int ForumCategoriesCount { get; set; }

        public IEnumerable<CategoryInForumViewModel> ForumCategories { get; set; }
    }
}