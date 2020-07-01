namespace GamersHub.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    public class ForumCategoryViewModel
    {
        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public ForumCategoryByNameViewModel ForumCategory { get; set; }

        public IEnumerable<PostInCategoryViewModel> CategoryPosts { get; set; }
    }
}
