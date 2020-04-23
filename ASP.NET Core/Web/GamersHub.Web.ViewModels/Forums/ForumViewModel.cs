namespace GamersHub.Web.ViewModels.Forums
{
    using System.Collections.Generic;

    using GamersHub.Common;
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class ForumViewModel : IMapFrom<Forum>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Name);

        public int PostsCount { get; set; }

        public IEnumerable<CategoryInForumIndexViewModel> ForumCategories { get; set; }
    }
}
