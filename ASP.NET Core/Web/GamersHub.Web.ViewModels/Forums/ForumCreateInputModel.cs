using System.ComponentModel.DataAnnotations;

namespace GamersHub.Web.ViewModels.Forums
{
    public class ForumCreateInputModel
    {
        [Required]
        public string Name { get; set; }
    }
}
