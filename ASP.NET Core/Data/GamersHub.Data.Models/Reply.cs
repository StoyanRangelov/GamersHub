namespace GamersHub.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using GamersHub.Data.Common.Models;

    public class Reply : BaseDeletableModel<int>
    {
        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
