using System;
using System.Linq;
using AutoMapper;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Home
{
    public class UserHomeIndexViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Username { get; set; }

        public DateTime CreatedOn { get; set; }

        public int PostsCount { get; set; }

        public int RepliesCount { get; set; }

        public int PartiesCount { get; set; }

        public int PositiveReviews { get; set; }

        public int NegativeReviews { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Game, UserHomeIndexViewModel>()
                .ForMember(x => x.NegativeReviews, y => y
                    .MapFrom(x => x.Reviews.Count(x => x.IsPositive == false)))
                .ForMember(x => x.PositiveReviews, y => y
                    .MapFrom(x => x.Reviews.Count(x => x.IsPositive)));
        }
    }
}