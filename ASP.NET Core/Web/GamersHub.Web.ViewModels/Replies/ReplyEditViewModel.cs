using System.ComponentModel.DataAnnotations;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Replies
{
    public class ReplyEditViewModel : IMapFrom<Reply>
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public int PostId { get; set; }

        public string PostUrl { get; set; }

        public ReplyPostViewModel Post { get; set; }
    }
}