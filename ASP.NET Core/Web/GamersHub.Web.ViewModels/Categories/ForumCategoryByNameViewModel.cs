using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Categories
{
    public class ForumCategoryByNameViewModel : IMapFrom<ForumCategory>
    {
        public string ForumName { get; set; }

        public string CategoryName { get; set; }
    }
}