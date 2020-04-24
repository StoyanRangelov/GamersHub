namespace GamersHub.Web.ViewModels.Pages.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class PrivacyPageEditInputModel : IMapFrom<Page>
    {
        [Required]
        public string Content { get; set; }
    }
}
