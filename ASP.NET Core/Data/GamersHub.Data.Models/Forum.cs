namespace GamersHub.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using GamersHub.Data.Common.Models;

    public class Forum : BaseDeletableModel<int>
    {
        public Forum()
        {
            this.ForumCategories = new HashSet<ForumCategory>();
            this.Posts = new HashSet<Post>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<ForumCategory> ForumCategories { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
