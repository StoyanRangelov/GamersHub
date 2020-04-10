using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Forums
{
    public class CategoryInForumByNameViewModel : IMapFrom<ForumCategory>
    {
        public string CategoryName { get; set; }

        public string Url => UrlParser.ParseToUrl(this.CategoryName);
    }
}