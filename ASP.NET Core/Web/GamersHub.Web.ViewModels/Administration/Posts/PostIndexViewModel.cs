using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Administration.Posts
{
    public class PostIndexViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}