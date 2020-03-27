using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamersHub.Data.Common.Models;

namespace GamersHub.Data.Models
{
   public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            CategoryForums = new HashSet<ForumCategory>();
            Posts = new HashSet<Post>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual ICollection<ForumCategory> CategoryForums { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
