using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Administration.Posts
{
    public class PostIndexViewModel
    {
        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}