using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
    public class GamesController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}