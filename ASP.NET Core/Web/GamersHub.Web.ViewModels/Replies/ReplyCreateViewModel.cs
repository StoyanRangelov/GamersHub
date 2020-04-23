namespace GamersHub.Web.ViewModels.Replies
{
    using System.ComponentModel.DataAnnotations;

    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class ReplyCreateViewModel : IMapFrom<Reply>
    {
        [Required]
        public string Content { get; set; }

        public int PostId { get; set; }

        public string PostUrl { get; set; }

        public ReplyPostViewModel Post { get; set; }
    }
}
