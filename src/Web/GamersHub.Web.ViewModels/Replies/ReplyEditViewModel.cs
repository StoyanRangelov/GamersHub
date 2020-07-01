namespace GamersHub.Web.ViewModels.Replies
{
    using System.ComponentModel.DataAnnotations;

    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class ReplyEditViewModel : IMapFrom<Reply>
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public string Content { get; set; }

        public string PostUrl { get; set; }

        public ReplyPostViewModel Post { get; set; }
    }
}
