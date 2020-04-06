using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Areas.Administration.Controllers
{
    public class ModeratorsController : AdministrationController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}