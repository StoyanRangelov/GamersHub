using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GamersHub.Data.Common.Models;

namespace GamersHub.Data.Models
{
    public class Party : BaseDeletableModel<int>
    {
        [Required]
        public string Game { get; set; }

        [Required]
        public string Activity { get; set; }

        [Required]
        public string Description { get; set; }

        public int Size { get; set; }

        public bool IsFull { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        public virtual ICollection<PartyApplicant> PartyApplicants { get; set; }
    }
}