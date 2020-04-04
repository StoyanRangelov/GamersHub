using System.Collections.Generic;

namespace GamersHub.Web.ViewModels.Administration.Forums
{
    public class ForumAdministrationEditInputModel
    {
        public int Id { get; set; }

        public ForumEditInputModel Forum { get; set; }

        public CategoryEditInputModel[] CategoriesInput { get; set; }

        public CategoryEditViewModel[] Categories { get; set; }
    }
}