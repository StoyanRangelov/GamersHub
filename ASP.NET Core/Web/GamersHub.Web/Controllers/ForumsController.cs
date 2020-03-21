﻿using System.Threading.Tasks;
using GamersHub.Services.Data;
using GamersHub.Web.ViewModels.Forums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
    [Authorize]
    public class ForumsController : BaseController
    {
        private readonly IForumsService forumsService;

        public ForumsController(IForumsService forumsService)
        {
            this.forumsService = forumsService;
        }

        public IActionResult Index()
        {
            var viewModel = new ForumIndexViewModel
            {
                Forums = this.forumsService.GetAll<ForumViewModel>(),
            };

            return this.View(viewModel);
        }

        public IActionResult ByName(string name, string id)
        {
            this.ViewData["CategoryName"] = id;

            var viewModel = this.forumsService.GetByName<ForumByNameViewModel>(name);

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Create(ForumCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.forumsService.CreateAsync(inputModel.Name);

            return this.Redirect("/Forums");
        }
    }
}
