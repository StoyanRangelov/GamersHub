using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Administration.Posts
{
    public class PostIndexViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}