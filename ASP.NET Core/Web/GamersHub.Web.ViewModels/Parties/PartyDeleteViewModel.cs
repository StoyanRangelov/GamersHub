using System;
using System.Collections.Generic;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Parties
{
    public class PartyDeleteViewModel : IMapFrom<Party>
    {
        public int Id { get; set; }

        public string Game { get; set; }

        public string Activity { get; set; }

        public string Description { get; set; }

        public int Size { get; set; }

        public string CreatorId { get; set; }

        public string CreatorUsername { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<PartyApplicantDeleteViewModel> PartyApplicants { get; set; }
    }
}