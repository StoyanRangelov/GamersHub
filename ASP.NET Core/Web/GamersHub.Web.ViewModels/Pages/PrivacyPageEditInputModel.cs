using System.ComponentModel.DataAnnotations;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Pages
{
    public class PrivacyPageEditInputModel : IMapFrom<Page>
    {
        [Required]
        public string Content { get; set; }
    }
}