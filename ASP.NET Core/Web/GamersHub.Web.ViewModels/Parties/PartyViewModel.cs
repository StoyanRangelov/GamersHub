using System;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Parties
{
    public class PartyViewModel : IMapFrom<Party>
    {
        public int Id { get; set; }

        public string CreatorId { get; set; }

        public string CreatorUsername { get; set; }

        public string Game { get; set; }

        public string Activity { get; set; }

        public string Description { get; set; }

        public int Size { get; set; }

        public int PartyApplicantsCount { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}