namespace GamersHub.Web.ViewModels.Forums
{
    using System.Collections.Generic;

    using GamersHub.Common;
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class ForumByNameViewModel : IMapFrom<Forum>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Name);

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<CategoryInForumByNameViewModel> ForumCategories { get; set; }

        public IEnumerable<PostInForumViewModel> ForumPosts { get; set; }
    }
}
