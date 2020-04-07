using Ganss.XSS;

namespace GamersHub.Web.ViewModels.Games
{
    public class ReviewInGameViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public string UserUsername { get; set; }

        public bool IsPositive { get; set; }
    }
}