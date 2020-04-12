using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GamersHub.Services.Data.Parties;
using GamersHub.Services.Data.Users;
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
        private readonly IUsersService usersService;

        public PartiesController(IPartiesService partiesService, IUsersService usersService)
        {
            this.partiesService = partiesService;
            this.usersService = usersService;
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

            var applyId = await this.partiesService.ApplyAsync(input.PartyId, userId);

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

        public IActionResult Host(string id, int page = 1)
        {
            var partyUserId = this.usersService.GetIdByName(id);
            if (partyUserId == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (partyUserId != currentUserId)
            {
                return this.Redirect("/Identity/Account/AccessDenied");
            }

            var viewModel = this.usersService.GetByName<PartyHostViewModel>(id);


            viewModel.UserParties = this.partiesService
                .GetAllByUsername<PartyWithApplicantsViewModel>(id, PartiesPerPage, (page - 1) * PartiesPerPage);


            var count = this.partiesService.GetCountByUsername(id);

            viewModel.PagesCount = (int) Math.Ceiling((double) count / PartiesPerPage);
            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(PartyApplicantInputModel input)
        {
            var approveUserId = await this.partiesService.ApproveAsync(input.PartyId, input.ApplicantId);

            if (approveUserId == 0)
            {
                return this.NotFound();
            }

            this.TempData["InfoMessage"] = "Successfully approved user to party.";
            return this.RedirectToAction("Host", "Parties", new {id = input.CreatorUsername});
        }
    }
}