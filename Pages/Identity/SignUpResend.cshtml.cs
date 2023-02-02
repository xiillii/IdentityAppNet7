using IdentityApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace IdentityApp.Pages.Identity
{
    [AllowAnonymous]
    public class SignUpResendModel : UserPageModel
    {
        public UserManager<IdentityUser> UserManager { get; set; }
        public IdentityEmailService EmailService { get; set; }

        [EmailAddress]
        [BindProperty(SupportsGet = true)]
        public string? Email { get; set; }

        public SignUpResendModel(UserManager<IdentityUser> userManager, IdentityEmailService emailService)
        {
            UserManager = userManager;
            EmailService = emailService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(Email);
                if (user != null && !await UserManager.IsEmailConfirmedAsync(user))
                {
                    await EmailService.SendAccountConfirmEmail(user, "SignUpConfirm");
                }
                TempData["message"] = "Confirmation email sent. Check your inbox.";
                return RedirectToPage(new { Email });
            }
            return Page();
        }
    }
}
