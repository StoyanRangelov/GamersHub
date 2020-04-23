namespace GamersHub.Web.ViewModels.Administration.Games
{
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;
    using Ganss.XSS;

    public class GameDeleteViewModel : IMapFrom<Game>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Description { get; set; }

        public int ReviewsCount { get; set; }

        public string SanitisedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public string ImgUrl { get; set; }
    }
}
