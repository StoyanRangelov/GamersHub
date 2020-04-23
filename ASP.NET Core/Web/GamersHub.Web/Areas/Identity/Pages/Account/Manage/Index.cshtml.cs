namespace GamersHub.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using GamersHub.Data.Models;
    using GamersHub.Services.Data.Users;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUsersService usersService;
        private readonly Cloudinary cloudinary;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUsersService usersService,
            Cloudinary cloudinary)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.usersService = usersService;
            this.cloudinary = cloudinary;
        }

        public string Username { get; set; }

        public string ImgUrl { get; set; }

        [Display(Name = "Current Gaming Experience")]
        [EnumDataType(typeof(GamingExperienceType))]
        public GamingExperienceType CurrentGamingExperience { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Change Gaming Experience")]
            [EnumDataType(typeof(GamingExperienceType))]
            public GamingExperienceType GamingExperience { get; set; }

            [Required]
            [StringLength(40, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 4)]
            [Display(Name = "Discord Username")]
            public string DiscordUsername { get; set; }

            public IFormFile Image { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await this.userManager.GetUserNameAsync(user);
            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);

            this.Username = userName;
            this.CurrentGamingExperience = user.GamingExperience;
            this.ImgUrl = user.ImgUrl;

            this.Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                DiscordUsername = user.DiscordUsername,
                GamingExperience = user.GamingExperience,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            var imageUrl = string.Empty;
            if (this.Input.Image != null)
            {
                var fileName = ContentDispositionHeaderValue.Parse(this.Input.Image.ContentDisposition).FileName.Trim('"');

                await using var stream = this.Input.Image.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fileName, stream),
                    Format = "jpg",
                };

                var uploadResult = await this.cloudinary.UploadAsync(uploadParams);

                imageUrl = uploadResult.SecureUri.ToString();
            }

            var id = await this.usersService.EditProfileAsync(user.Id, this.Input.DiscordUsername, this.Input.GamingExperience, imageUrl);
            if (id == null)
            {
                return this.NotFound();
            }

            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await this.userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException(
                        $"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }
    }
}
