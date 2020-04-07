using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;
using Ganss.XSS;

namespace GamersHub.Web.ViewModels.Games
{
    public class GameByIdViewModel : IMapFrom<Game>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Description { get; set; }

        public int PositiveReviews { get; set; }

        public int NegativeReviews { get; set; }

        public string SanitisedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public string ImgUrl { get; set; }

        public IEnumerable<ReviewInGameViewModel> Reviews { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Game, GameByIdViewModel>()
                .ForMember(x => x.NegativeReviews, y => y
                    .MapFrom(x => x.Reviews.Count(x => x.IsPositive == false)))
                .ForMember(x=>x.PositiveReviews, y => y
                    .MapFrom(x=>x.Reviews.Count(x=>x.IsPositive)));
        }
    }
}