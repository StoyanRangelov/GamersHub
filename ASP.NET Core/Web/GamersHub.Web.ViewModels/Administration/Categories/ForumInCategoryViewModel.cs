using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Categories
{
    public class ForumInCategoryViewModel : IMapFrom<ForumCategory>
    {
        public string ForumName { get; set; }
    }
}