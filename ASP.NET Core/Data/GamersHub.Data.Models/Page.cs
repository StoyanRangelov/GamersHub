namespace GamersHub.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using GamersHub.Data.Common.Models;

    public class Page : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
