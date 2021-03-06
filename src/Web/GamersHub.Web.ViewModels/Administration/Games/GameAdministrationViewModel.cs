﻿namespace GamersHub.Web.ViewModels.Administration.Games
{
    using System.Linq;

    using AutoMapper;
    using GamersHub.Common;
    using GamersHub.Data.Models;
    using GamersHub.Services.Mapping;

    public class GameAdministrationViewModel : IMapFrom<Game>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Url => UrlParser.ParseToUrl(this.Title);

        public int ReviewsCount { get; set; }

        public int PositiveReviews { get; set; }

        public int NegativeReviews { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Game, GameAdministrationViewModel>()
                .ForMember(x => x.NegativeReviews, y => y
                    .MapFrom(x => x.Reviews.Count(r => r.IsPositive == false)))
                .ForMember(x => x.PositiveReviews, y => y
                    .MapFrom(x => x.Reviews.Count(r => r.IsPositive)));
        }
    }
}
