namespace GamersHub.Web.Areas.Identity.Pages.Account.Manage
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using GamersHub.Common;
    using GamersHub.Data.Models;
    using GamersHub.Services.Data.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<DeletePersonalDataModel> logger;
        private readonly IUsersService usersService;

        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            IUsersService usersService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.usersService = usersService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            this.RequirePassword = await this.userManager.HasPasswordAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            this.RequirePassword = await this.userManager.HasPasswordAsync(user);
            if (this.RequirePassword)
            {
                if (!await this.userManager.CheckPasswordAsync(user, this.Input.Password))
                {
                    this.ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return this.Page();
                }
            }

            var isAdministrator = await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName);
            if (isAdministrator)
            {
                await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.AdministratorRoleName);
            }

            var isModerator = await this.userManager.IsInRoleAsync(user, GlobalConstants.ModeratorRoleName);
            if (isModerator)
            {
                await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.ModeratorRoleName);
            }

            var userlogins = await this.userManager.GetLoginsAsync(user);
            foreach (var userlogin in userlogins)
            {
               await this.userManager.RemoveLoginAsync(user, userlogin.LoginProvider, userlogin.ProviderKey);
            }

            await this.usersService.DeleteAsync(user.Id);

            await this.signInManager.SignOutAsync();

            this.logger.LogInformation("User with ID '{UserId}' deleted themselves.", user.Id);

            this.TempData["InfoMessage"] = "You have successfully deleted your account.";
            return this.Redirect("~/");
        }
    }
}
