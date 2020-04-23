namespace GamersHub.Web.ViewModels.Administration.Categories
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class ForumInCategoryDeleteViewModel : IMapFrom<ForumCategory>
    {
        public string ForumName { get; set; }
    }
}
