using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Replies
{
    public class ReplyInputModel : IMapFrom<Reply>
    {
        public int PostId { get; set; }

        public string Content { get; set; }
    }
}