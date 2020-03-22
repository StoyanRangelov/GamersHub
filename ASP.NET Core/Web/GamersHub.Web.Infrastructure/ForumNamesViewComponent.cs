using System.Threading.Tasks;
using GamersHub.Services.Data;
using GamersHub.Web.ViewModels.Forums;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Infrastructure
{
    public class ForumNamesViewComponent : ViewComponent
    {
        private readonly IForumsService forumsService;

        public ForumNamesViewComponent(IForumsService forumsService)
        {
            this.forumsService = forumsService;
        }

        public IViewComponentResult Invoke()
        {
            var forumNames = this.forumsService.GetAllNames();

            return this.View(forumNames);
        }
    }
}
