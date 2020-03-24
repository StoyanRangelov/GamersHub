namespace GamersHub.Web.ViewModels.Posts
{
    public class ReplyInPostViewModel
    {
        public string Content { get; set; }

        public string UserUsername { get; set; }

        public int UserPostsCount { get; set; }

        public int UserRepliesCount { get; set; }

        public int Publications => this.UserPostsCount + this.UserRepliesCount;
    }
}