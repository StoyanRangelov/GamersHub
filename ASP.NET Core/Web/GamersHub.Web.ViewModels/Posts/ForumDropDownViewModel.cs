using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Posts
{
    public class ForumDropDownViewModel : IMapFrom<Forum>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}