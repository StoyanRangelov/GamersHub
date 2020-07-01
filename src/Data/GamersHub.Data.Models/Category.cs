namespace GamersHub.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using GamersHub.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.CategoryForums = new HashSet<ForumCategory>();
            this.Posts = new HashSet<Post>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual ICollection<ForumCategory> CategoryForums { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
