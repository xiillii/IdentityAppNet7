using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity.Admin;

public class ViewClaimsPrincipalModel : AdminPageModel
{
    public UserManager<IdentityUser> UserManager { get; set; }
    public SignInManager<IdentityUser> SignInManager { get; set; }

    [BindProperty(SupportsGet = true)] public string? Id { get; set; }
    [BindProperty(SupportsGet = true)] public string? Callback { get; set; }

    public ClaimsPrincipal? Principal { get; set; }

    public ViewClaimsPrincipalModel(UserManager<IdentityUser> mng, SignInManager<IdentityUser> sng)
    {
        UserManager = mng;
        SignInManager = sng;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (string.IsNullOrEmpty(Id))
        {
            return RedirectToPage("SelectUser", new { Label = "View ClaimsPrincipal", Callback = "ClaimsPrincipal" });
        }
        var user = await UserManager.FindByIdAsync(Id);
        Principal = await SignInManager.CreateUserPrincipalAsync(user);

        return Page();
    }
}