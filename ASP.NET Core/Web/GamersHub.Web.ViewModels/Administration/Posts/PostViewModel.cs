using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Posts
{
    public class PostViewModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Name).ToLower();

        public string ShortName =>
            this.Name?.Length > 30
                ? this.Name?.Substring(0, 30) + "..."
                : this.Name;

        public string UserUsername { get; set; }

        public string ShortUsername =>
            this.UserUsername?.Length > 10
                ? this.UserUsername?.Substring(0, 10) + "..."
                : this.UserUsername;

        public string ForumName { get; set; }

        public string ShortForumName =>
            this.ForumName?.Length > 20
                ? this.ForumName?.Substring(0, 20) + "..."
                : this.ForumName;

        public string CategoryName { get; set; }

        public string ShortCategoryName =>
            this.CategoryName?.Length > 10
                ? this.CategoryName?.Substring(0, 10) + "..."
                : this.CategoryName;

        public int RepliesCount { get; set; }

    }
}