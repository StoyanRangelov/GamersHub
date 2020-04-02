using System.Collections.Generic;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Categories
{
    public class CategoryDeleteViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int PostsCount { get; set; }

        public int CategoryForumsCount { get; set; }

        public IEnumerable<ForumInCategoryDeleteViewModel> CategoryForums { get; set; }
    }
}