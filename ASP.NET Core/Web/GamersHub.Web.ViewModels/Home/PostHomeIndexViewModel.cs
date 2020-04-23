namespace GamersHub.Web.ViewModels.Home
{
    using System;

    using GamersHub.Common;
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class PostHomeIndexViewModel : IMapFrom<Post>
    {
        public string Name { get; set; }

        public string ShortName =>
            this.Name?.Length > 40
                ? this.Name?.Substring(0, 40) + "..."
                : this.Name;

        public string Url => UrlParser.ParseToUrl(this.Name).ToLower();

        public string UserUsername { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ForumName { get; set; }

        public string CategoryName { get; set; }

        public string RepliesCount { get; set; }
    }
}
