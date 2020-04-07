using System.Collections.Generic;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using Ganss.XSS;

namespace GamersHub.Web.ViewModels.Games
{
    public class GameByIdViewModel : IMapFrom<Game>
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Description { get; set; }

        public string SanitisedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public string ImgUrl { get; set; }

        public IEnumerable<ReviewInGameViewModel> Reviews { get; set; }
    }
}