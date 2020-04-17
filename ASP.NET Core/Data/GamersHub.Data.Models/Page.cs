using System.ComponentModel.DataAnnotations;
using GamersHub.Data.Common.Models;
using GamersHub.Data.Common.Repositories;

namespace GamersHub.Data.Models
{
    public class Page : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }
    }
}