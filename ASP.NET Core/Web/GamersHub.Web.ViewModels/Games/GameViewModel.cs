using System.Linq;
using AutoMapper;
using GamersHub.Common;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Games
{
    public class GameViewModel : IMapFrom<Game>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Title);

        public string ImgUrl { get; set; }

        public string SubTitle { get; set; }

        public int PositiveReviews { get; set; }

        public int NegativeReviews { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Game, GameViewModel>()
                .ForMember(x => x.PositiveReviews, y => y
                    .MapFrom(x => x.Reviews.Count(x => x.IsPositive)));

            configuration.CreateMap<Game, GameViewModel>()
                .ForMember(x => x.NegativeReviews, y => y
                    .MapFrom(x => x.Reviews.Count(x => x.IsPositive == false)));
        }
    }
}