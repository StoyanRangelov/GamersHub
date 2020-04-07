using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Areas.Administration.Controllers
{
    public class GamesController : AdministrationController
    {
        private readonly Cloudinary cloudinary;

        public GamesController(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public IActionResult Create()
        {


            return this.View();
        }
    }
}