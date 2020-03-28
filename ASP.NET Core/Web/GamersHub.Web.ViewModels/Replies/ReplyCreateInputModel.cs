using System.ComponentModel.DataAnnotations;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Replies
{
    public class ReplyCreateInputModel : IMapFrom<Reply>
    {
        [Required]
        [Display(Name = "Reply")]
        public string Content { get; set; }

        public int PostId { get; set; }

        public string PostUrl { get; set; }

        public ReplyPostCreateViewModel Post { get; set; }
    }
}