namespace GamersHub.Web.ViewModels.Administration.Posts
{
    using System.Collections.Generic;

    public class PostIndexViewModel
    {
        public PaginationViewModel Pagination { get; set; }

        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}
