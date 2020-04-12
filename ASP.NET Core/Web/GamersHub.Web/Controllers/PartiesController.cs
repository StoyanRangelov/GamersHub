using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GamersHub.Services.Data.Parties;
using GamersHub.Web.ViewModels;
using GamersHub.Web.ViewModels.Parties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{

    [Authorize]
    public class PartiesController : BaseController
    {
        private const int PartiesPerPage = 6;
        
        private readonly IPartiesService partiesService;

        public PartiesController(IPartiesService partiesService)
        {
            this.partiesService = partiesService;
        }

        public IActionResult Index(int id = 1)
        {
            var parties = this.partiesService
                .GetAll<PartyViewModel>(PartiesPerPage, (id - 1) * PartiesPerPage);

            var viewModel = new PartyIndexViewModel {Parties = parties};

            var count = this.partiesService.GetCount();

            var pagination = new PaginationViewModel();

            pagination.PagesCount = (int) Math.Ceiling((double) count / PartiesPerPage);
            if (pagination.PagesCount == 0)
            {
                pagination.PagesCount = 1;
            }

            pagination.CurrentPage = id;

            viewModel.Pagination = pagination;

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PartyCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var partyId = await this.partiesService.CreateAsync(
                userId,
                input.Game,
                input.Activity,
                input.Description,
                input.Size);

            this.TempData["InfoMessage"] = "Party created successfully!";
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> Apply(PartyApplyInputModel input)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == input.UserId)
            {
                this.TempData["InfoMessage"] = "You cannot apply to a party you created.";
                return this.RedirectToAction(nameof(this.Index));
            }

            var applyId = await this.partiesService.ApplyAsync(input.PartyId, input.UserId);

            if (applyId == 0)
            {
                return this.NotFound();
            }

            if (applyId == -1)
            {
                this.TempData["InfoMessage"] = "You have already applied to this party.";
                return this.RedirectToAction(nameof(this.Index));
            }

            this.TempData["InfoMessage"] = "Successfully applied to party.";
            return this.RedirectToAction(nameof(this.Index));
        }

    }
}
