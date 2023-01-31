using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IdentityApp.Pages.Identity
{
    [AllowAnonymous]
    public class SignInModel : UserPageModel
    {
        public SignInManager<IdentityUser> SignInManager { get; set; }

        [Required]
        [EmailAddress]
        [BindProperty]
        public string? Email { get; set; }

        [Required] [BindProperty] public string? Password { get; set; }

        [BindProperty(SupportsGet = true)] public string? ReturnUrl { get; set; }

        public SignInModel(SignInManager<IdentityUser> mgr)
        {
            SignInManager = mgr;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = await SignInManager.PasswordSignInAsync(Email, Password, true, true);
            if (result.Succeeded)
            {
                return Redirect(ReturnUrl ?? "/");
            }

            if (result.IsLockedOut)
            {
                TempData["message"] = "Account Locked";
            } else if (result.IsNotAllowed)
            {
                TempData["message"] = "Sign In Not Allowed";
            } else if (result.RequiresTwoFactor)
            {
                return RedirectToPage("SignInTwoFactor", new { ReturnUrl });
            }
            else
            {
                TempData["message"] = "Sign In Failed";
            }

            return Page();
        }
    }
}
