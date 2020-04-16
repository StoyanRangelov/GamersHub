using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Categories
{
    public class ForumEditViewModel : IMapFrom<Forum>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}