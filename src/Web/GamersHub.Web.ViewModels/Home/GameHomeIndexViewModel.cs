﻿namespace GamersHub.Web.ViewModels.Home
{
    using System.Linq;

    using AutoMapper;
    using GamersHub.Common;
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class GameHomeIndexViewModel : IMapFrom<Game>, IHaveCustomMappings
    {
        public string Title { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Title);

        public string SubTitle { get; set; }

        public int ReviewsCount { get; set; }

        public int PositiveReviews { get; set; }

        public int NegativeReviews { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Game, GameHomeIndexViewModel>()
                .ForMember(x => x.NegativeReviews, y => y
                    .MapFrom(x => x.Reviews.Count(x => x.IsPositive == false)))
                .ForMember(x => x.PositiveReviews, y => y
                    .MapFrom(x => x.Reviews.Count(x => x.IsPositive)));
        }
    }
}
