using IdentityApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IdentityApp.Pages.Identity;

[AllowAnonymous]
public class UserPasswordRecoveryConfirmModel : UserPageModel
{
    public UserManager<IdentityUser> UserManager { get; set; }
    public TokenUrlEncoderService TokenUrlEncoder { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Email { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Token { get; set; }

    [BindProperty]
    [Required]
    public string? Password { get; set; }

    [BindProperty]
    [Required]
    [Compare(nameof(Password))]
    public string? ConfirmPassword { get; set; }

    public UserPasswordRecoveryConfirmModel(UserManager<IdentityUser> mgr, TokenUrlEncoderService service)
    {
        UserManager = mgr;
        TokenUrlEncoder = service;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = await UserManager.FindByEmailAsync(Email);
            var decodedToken = TokenUrlEncoder.DecodeToken(Token);
            var result = await UserManager.ResetPasswordAsync(user, decodedToken, Password);

            if (result.Process(ModelState))
            {
                TempData["message"] = "Password changed";
                return RedirectToPage();
            }
        }

        return Page();
    }
}