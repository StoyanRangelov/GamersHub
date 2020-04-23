namespace GamersHub.Web.ViewModels.Forums
{
    using GamersHub.Common;
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class CategoryInForumByNameViewModel : IMapFrom<ForumCategory>
    {
        public string CategoryName { get; set; }

        public string Url => UrlParser.ParseToUrl(this.CategoryName);
    }
}
