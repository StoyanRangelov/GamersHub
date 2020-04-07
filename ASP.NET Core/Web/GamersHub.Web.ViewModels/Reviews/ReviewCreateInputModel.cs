using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GamersHub.Web.ViewModels.Reviews
{
    public class ReviewCreateInputModel
    {
        [Display(Name = "Game")]
        [Range(1, int.MaxValue)]
        public int GameId { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsPositive { get; set; }
        
        public IEnumerable<GameDropDownViewModel> Games { get; set; }
    }
}