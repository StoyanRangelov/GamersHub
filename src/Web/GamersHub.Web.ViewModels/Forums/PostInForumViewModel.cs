namespace GamersHub.Web.ViewModels.Forums
{
    using System;

    using GamersHub.Common;
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class PostInForumViewModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Name).ToLower();

        public string ShortName =>
            this.Name?.Length > 60
                ? this.Name?.Substring(0, 60) + "..."
                : this.Name;

        public string CategoryName { get; set; }

        public string UserUsername { get; set; }

        public int RepliesCount { get; set; }
    }
}
