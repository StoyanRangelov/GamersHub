using AutoMapper;
using GamersHub.Data.Models;
using GamersHub.Services.Mapping;

namespace GamersHub.Web.ViewModels.Administration.Categories
{
   public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ShortDescription =>
            this.Description?.Length > 60
                ? this.Description?.Substring(0, 60) + "..."
                : this.Description;

        public int CategoryForumsCount { get; set; }

        public int PostsCount { get; set; }
       
    }
}
