using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamersHub.Data.Common.Models;

namespace GamersHub.Data.Models
{
    public class Game : BaseDeletableModel<int>
    {
        public Game()
        {
            this.Reviews = new HashSet<Review>();
        }

        [Required]
        public string Title { get; set; }

        public string SubTitle { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImgUrl { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}