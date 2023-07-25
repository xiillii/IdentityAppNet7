using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace IdentityApp.Pages.Identity;

[AllowAnonymous]
public class SignInModel : UserPageModel
{
    public SignInManager<IdentityUser> SignInManager { get; set; }
    public UserManager<IdentityUser> UserManager { get; set; }

    [Required]
    [EmailAddress]
    [BindProperty]
    public string? Email { get; set; }

    [Required][BindProperty] public string? Password { get; set; }

    [BindProperty(SupportsGet = true)] public string? ReturnUrl { get; set; }

    public SignInModel(SignInManager<IdentityUser> mgr, UserManager<IdentityUser> manager)
    {
        SignInManager = mgr;
        UserManager = manager;
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
        }
        else if (result.IsNotAllowed)
        {
            var user = await UserManager.FindByEmailAsync(Email);
            if (user != null && !await UserManager.IsEmailConfirmedAsync(user))
            {
                return RedirectToPage("SignUpConfirm");
            }

            TempData["message"] = "Sign In Not Allowed";
        }
        else if (result.RequiresTwoFactor)
        {
            return RedirectToPage("SignInTwoFactor", new { ReturnUrl });
        }
        else
        {
            TempData["message"] = "Sign In Failed";
        }

        return Page();
    }

    public IActionResult OnPostExternalAsync(string provider)
    {
        var callbackUrl = Url.Page("SignIn", "Callback", new { ReturnUrl });
        var props = SignInManager.ConfigureExternalAuthenticationProperties(provider
            , callbackUrl);

        return new ChallengeResult(provider, props);
    }

    public async Task<IActionResult> OnGetCallbackAsync()
    {
        var info = await SignInManager.GetExternalLoginInfoAsync();
        var result = await SignInManager.ExternalLoginSignInAsync(
            info.LoginProvider, info.ProviderKey, true);

        if (result.Succeeded)
        {
            return Redirect(WebUtility.UrlDecode(ReturnUrl ?? "//"));
        }
        else if (result.IsLockedOut)
        {
            TempData["message"] = "Account Locked";
        }
        else if (result.IsNotAllowed)
        {
            TempData["message"] = "Sign In Not Allowed";
        }
        else
        {
            TempData["message"] = "Sign In Failed";
        }

        return RedirectToPage();
    }
}

