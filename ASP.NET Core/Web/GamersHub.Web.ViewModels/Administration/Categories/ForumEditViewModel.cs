using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Categories
{
    public class ForumEditViewModel : IMapFrom<ForumCategory>
    {
        public int ForumId { get; set; }

        public string ForumName { get; set; }
    }
}