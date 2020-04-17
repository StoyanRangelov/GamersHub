using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using Ganss.XSS;

namespace GamersHub.Web.ViewModels.Pages
{
    public class PrivacyPageViewModel : IMapFrom<Page>
    {
        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);
    }
}