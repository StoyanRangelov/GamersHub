using System;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Parties
{
    public class PartyAdministrationViewModel : IMapFrom<Party>
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public int Size { get; set; }

        public string Game { get; set; }

        public string CreatorUsername { get; set; }

        public int PartyApplicantsCount { get; set; }
    }
}