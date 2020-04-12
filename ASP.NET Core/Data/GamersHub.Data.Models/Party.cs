﻿using System.Collections;
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

        public int Capacity { get; set; }

        public bool IsFull { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        public virtual ICollection<PartyUser> PartyApplicants { get; set; }
    }
}