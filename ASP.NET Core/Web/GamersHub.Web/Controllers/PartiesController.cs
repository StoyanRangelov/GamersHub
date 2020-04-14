using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GamersHub.Common;
using GamersHub.Services.Data.Parties;
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


            viewModel.PartyApplications = this.partiesService
                .GetAllApplicationsByUsername<ApplicantPartiesViewModel>(id, PartiesPerPage, (page - 1) * PartiesPerPage);


            var count = this.partiesService.GetApplicationsCountByUsername(id);

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

        public async Task<IActionResult> Decline(PartyApplicantInputModel input)
        {
            var declineUserId = await this.partiesService.DeclineAsync(input.PartyId, input.ApplicantId);

            if (declineUserId == 0)
            {
                return this.NotFound();
            }

            this.TempData["InfoMessage"] = "Successfully declined party applicant";
            return this.RedirectToAction("Host", "Parties", new {id = input.CreatorUsername});
        }

        public async Task<IActionResult> CancelApplication(PartyApplicantInputModel input)
        {
           var partyId = await this.partiesService.CancelApplicationAsync(input.PartyId, input.ApplicantId);

           if (partyId == 0)
           {
               return this.NotFound();
           }

           this.TempData["InfoMessage"] = "Successfully canceled party application";
           return this.RedirectToAction("Applications", "Parties", new {id = input.CreatorUsername});
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

            var partyId = await this.partiesService.EditAsync(input.Id, input.Game, input.Activity, input.Description);

            if (partyId == 0)
            {
                return this.NotFound();
            }

            this.TempData["InfoMessage"] = "Party edited successfully!";
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) && this.User.Identity.Name != input.CreatorUsername)
            {
                return this.Redirect("/Administration/Parties/Index");
            }

            return this.RedirectToAction("Host", "Parties", new {id = input.CreatorUsername});
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

            await this.partiesService.DeleteAsync(input.PartyId);

            this.TempData["InfoMessage"] = "Party deleted successfully!";
            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName) && this.User.Identity.Name != input.CreatorUsername)
            {
                return this.Redirect("/Administration/Parties/Index");
            }
            return this.RedirectToAction("Host", "Parties", new { id = input.CreatorUsername});
        }
    }
}