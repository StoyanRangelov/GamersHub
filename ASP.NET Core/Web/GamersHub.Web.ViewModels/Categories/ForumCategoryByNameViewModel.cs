namespace GamersHub.Web.ViewModels.Categories
{
    using GamersHub.Common;
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class ForumCategoryByNameViewModel : IMapFrom<ForumCategory>
    {
        public int ForumId { get; set; }

        public string ForumName { get; set; }

        public string CategoryName { get; set; }

        public string CategoryUrl => UrlParser.ParseToUrl(this.CategoryName);
    }
}
