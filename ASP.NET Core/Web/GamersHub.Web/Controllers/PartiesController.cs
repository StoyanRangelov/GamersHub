using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Services.Data.Parties;
using GamersHub.Services.Data.PartyApplicants;
using GamersHub.Services.Data.Users;
using GamersHub.Web.ViewModels;
using GamersHub.Web.ViewModels.Parties;
using GamersHub.Web.ViewModels.Replies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamersHub.Web.Controllers
{
    [Authorize]
    public class PartiesController : BaseController
    {
        private const int PartiesPerPage = 6;

        private readonly IPartiesService partiesService;
        private readonly IPartyApplicantsService partyApplicantsService;
        private readonly IUsersService usersService;

        public PartiesController(IPartiesService partiesService, IUsersService usersService,
            IPartyApplicantsService partyApplicantsService)
        {
            this.partiesService = partiesService;
            this.usersService = usersService;
            this.partyApplicantsService = partyApplicantsService;
        }

        public IActionResult Index(string searchString, string currentFilter, int id = 1)
        {
            var parties = this.partiesService
                .GetAll<PartyViewModel>(PartiesPerPage, (id - 1) * PartiesPerPage, searchString);

            var viewModel = new PartyIndexViewModel {Parties = parties};

            viewModel.CurrentPage = id;

            if (searchString != null)
            {
                viewModel.CurrentPage = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;


            var count = this.partiesService.GetCount(searchString);
            viewModel.PagesCount = (int) Math.Ceiling((double) count / PartiesPerPage);
            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

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

            var partyId = await this.partiesService.ApplyAsync(input.PartyId, userId);

            if (partyId == null)
            {
                return this.NotFound();
            }

            if (partyId == 0)
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
                return this.BadRequest();
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

        public IActionResult Applications(string id, int page = 1)
        {
            var partyUserId = this.usersService.GetIdByName(id);
            if (partyUserId == null)
            {
                return this.NotFound();
            }

            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (partyUserId != currentUserId)
            {
                return this.BadRequest();
            }

            var viewModel = this.usersService.GetByName<ApplicantPartyViewModel>(id);


            viewModel.PartyApplications = this.partyApplicantsService
                .GetAllApplicationsByUsername<ApplicantPartiesViewModel>(id, PartiesPerPage,
                    (page - 1) * PartiesPerPage);


            var count = this.partyApplicantsService.GetApplicationsCountByUsername(id);

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
            var approveUserId = await this.partyApplicantsService.ApproveAsync(input.PartyId, input.ApplicantId);

            if (approveUserId == null)
            {
                return this.NotFound();
            }

            this.TempData["InfoMessage"] = "Successfully approved user to party.";
            return this.RedirectToAction("Host", "Parties", new {id = this.User.Identity.Name});
        }

        public async Task<IActionResult> Decline(PartyApplicantInputModel input)
        {
            var declineUserId = await this.partyApplicantsService.DeclineAsync(input.PartyId, input.ApplicantId);

            if (declineUserId == null)
            {
                return this.NotFound();
            }

            this.TempData["InfoMessage"] = "Successfully declined party applicant";
            return this.RedirectToAction("Host", "Parties", new {id = this.User.Identity.Name});
        }

        public async Task<IActionResult> CancelApplication(PartyApplicantInputModel input)
        {
            var partyId = await this.partyApplicantsService.CancelApplicationAsync(input.PartyId, input.ApplicantId);

            if (partyId == null)
            {
                return this.NotFound();
            }

            this.TempData["InfoMessage"] = "Successfully canceled party application";
            return this.RedirectToAction("Applications", "Parties", new {id = this.User.Identity.Name});
        }

        public IActionResult Edit(int id)
        {
            var viewModel = this.partiesService.GetById<PartyEditViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            if (!this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId != viewModel.CreatorId)
                {
                    return this.BadRequest();
                }
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PartyEditViewModel input)
        {
            if (!this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId != input.CreatorId)
                {
                    return this.BadRequest();
                }
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var partyId = await this.partiesService.EditAsync(input.Id, input.Game, input.ChangeActivity, input.Description);

            if (partyId == null)
            {
                return this.NotFound();
            }

            this.TempData["InfoMessage"] = "Party edited successfully!";
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) &&
                this.User.Identity.Name != input.CreatorUsername)
            {
                return this.Redirect("/Administration/Parties/Index");
            }

            return this.RedirectToAction("Host", "Parties", new {id = this.User.Identity.Name});
        }

        public IActionResult Delete(int id)
        {
            var viewModel = this.partiesService.GetById<PartyDeleteViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            if (!this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId != viewModel.CreatorId)
                {
                    return this.BadRequest();
                }
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PartyDeleteInputModel input)
        {
            if (!this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId != input.CreatorId)
                {
                    return this.BadRequest();
                }
            }

            var partyId = await this.partiesService.DeleteAsync(input.PartyId);
            if (partyId == null)
            {
                return this.NotFound();
            }

            this.TempData["InfoMessage"] = "Party deleted successfully!";
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) &&
                this.User.Identity.Name != input.CreatorUsername)
            {
                return this.Redirect("/Administration/Parties/Index");
            }

            return this.RedirectToAction("Host", "Parties", new {id = input.CreatorUsername});
        }
    }
}