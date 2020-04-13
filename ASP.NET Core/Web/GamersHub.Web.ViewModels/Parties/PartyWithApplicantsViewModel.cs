using System;
using System.Collections.Generic;
using System.Linq;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Parties
{
    public class PartyWithApplicantsViewModel : IMapFrom<Party>
    {
        public int Id { get; set; }

        public string Game { get; set; }

        public int AvailableSlots
        {
            get
            {
                var approvedApplicants = this.PartyApplicants
                    .Count(x => x.IsApproved);
                var result = this.Size - approvedApplicants;

                if (result < 0)
                {
                    return 0;
                }

                return result;
            }
        }

        public string Activity { get; set; }

        public string Description { get; set; }

        public int Size { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<PartyApplicantViewModel> PartyApplicants { get; set; }
    }
}