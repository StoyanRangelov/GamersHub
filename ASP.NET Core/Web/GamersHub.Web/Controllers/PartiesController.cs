namespace GamersHub.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using GamersHub.Common;
    using GamersHub.Data.Models;
    using GamersHub.Services.Data.Parties;
    using GamersHub.Services.Data.PartyApplicants;
    using GamersHub.Services.Data.Users;
    using GamersHub.Services.Messaging;
    using GamersHub.Web.ViewModels;
    using GamersHub.Web.ViewModels.Parties;
    using GamersHub.Web.ViewModels.Replies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class PartiesController : BaseController
    {
        private const int PartiesPerPage = 6;

        private readonly IPartiesService partiesService;
        private readonly IPartyApplicantsService partyApplicantsService;
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailSender;

        public PartiesController(
            IPartiesService partiesService,
            IUsersService usersService,
            IPartyApplicantsService partyApplicantsService,
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager)
        {
            this.partiesService = partiesService;
            this.usersService = usersService;
            this.partyApplicantsService = partyApplicantsService;
            this.emailSender = emailSender;
            this.userManager = userManager;
        }

        public IActionResult Index(string searchString, string currentFilter, int id = 1)
        {
            var parties = this.partiesService
                .GetAll<PartyViewModel>(PartiesPerPage, (id - 1) * PartiesPerPage, searchString);

            var viewModel = new PartyIndexViewModel { Parties = parties };

            viewModel.CurrentPage = id;

            if (searchString != null)
            {
                viewModel.CurrentPage = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            this.ViewData["CurrentFilter"] = searchString;

            var count = this.partiesService.GetCount(searchString);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / PartiesPerPage);
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

            viewModel.PagesCount = (int)Math.Ceiling((double)count / PartiesPerPage);
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
                .GetAllApplicationsByUsername<ApplicantPartiesViewModel>(id, PartiesPerPage, (page - 1) * PartiesPerPage);

            var count = this.partyApplicantsService.GetApplicationsCountByUsername(id);

            viewModel.PagesCount = (int)Math.Ceiling((double)count / PartiesPerPage);
            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(PartyApplicantApproveInputModel input)
        {
            var approveUserId = await this.partyApplicantsService.ApproveAsync(input.PartyId, input.ApplicantId);

            if (approveUserId == null)
            {
                return this.NotFound();
            }

            var user = await this.userManager.FindByIdAsync(input.ApplicantId);
            var username = await this.userManager.GetUserNameAsync(user);
            var email = await this.userManager.GetEmailAsync(user);

            var callbackUrl = this.Url.Action(
                "Applications",
                "Parties",
                values: new { id = username },
                protocol: this.Request.Scheme);

            var currentUsername = this.User.Identity.Name;

            await this.emailSender.SendEmailAsync(
                GlobalConstants.EmailSenderFrom,
                GlobalConstants.SystemName,
                email,
                "Party application approved",
                $"You application to {currentUsername}'s party has been approved. You can view your applications by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            this.TempData["InfoMessage"] = "Successfully approved user to party. A notification email has been send to him/her.";
            return this.RedirectToAction("Host", "Parties", new { id = this.User.Identity.Name });
        }

        public async Task<IActionResult> Decline(PartyApplicantApproveInputModel input)
        {
            var declineUserId = await this.partyApplicantsService.DeclineAsync(input.PartyId, input.ApplicantId);

            if (declineUserId == null)
            {
                return this.NotFound();
            }

            this.TempData["InfoMessage"] = "Successfully declined party applicant";
            return this.RedirectToAction("Host", "Parties", new { id = this.User.Identity.Name });
        }

        public async Task<IActionResult> CancelApplication(PartyApplicantApproveInputModel input)
        {
            var partyId = await this.partyApplicantsService.CancelApplicationAsync(input.PartyId, input.ApplicantId);

            if (partyId == null)
            {
                return this.NotFound();
            }

            this.TempData["InfoMessage"] = "Successfully canceled party application";
            return this.RedirectToAction("Applications", "Parties", new { id = this.User.Identity.Name });
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

            var partyId =
                await this.partiesService.EditAsync(input.Id, input.Game, input.ChangeActivity, input.Description);

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

            return this.RedirectToAction("Host", "Parties", new { id = this.User.Identity.Name });
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

            return this.RedirectToAction("Host", "Parties", new { id = input.CreatorUsername });
        }
    }
}
