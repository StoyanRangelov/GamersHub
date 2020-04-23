namespace GamersHub.Web.ViewModels.Pages
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;
    using Ganss.XSS;

    public class PrivacyPageViewModel : IMapFrom<Page>
    {
        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);
    }
}
