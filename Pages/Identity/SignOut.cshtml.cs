using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity;

[AllowAnonymous]
public class SignOutModel : UserPageModel
{
    public SignInManager<IdentityUser> SignInManager { get; set; }

    public SignOutModel(SignInManager<IdentityUser> mgr)
    {
        SignInManager = mgr;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await SignInManager.SignOutAsync();

        return RedirectToPage();
    }
}