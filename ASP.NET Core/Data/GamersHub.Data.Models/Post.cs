using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamersHub.Data.Common.Models;

namespace GamersHub.Data.Models
{
    public class Post : BaseDeletableModel<int>
    {
        public Post()
        {
            this.Replies = new HashSet<Reply>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int ForumId { get; set; }

        public virtual Forum Forum { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Reply> Replies { get; set; }

    }
}
