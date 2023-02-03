using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity.Admin;

public class ClaimsModel : PageModel
{
    public UserManager<IdentityUser> UserManager { get; set; }

    [BindProperty(SupportsGet = true)] public string? Id { get; set; }

    public IEnumerable<Claim>? Claims { get; set; }

    public ClaimsModel(UserManager<IdentityUser> mng)
    {
        UserManager = mng;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (string.IsNullOrEmpty(Id))
        {
            return RedirectToPage("SelectUser", new { Label = "Manage Claims", Callback = "Claims" });
        }

        var user = await UserManager.FindByIdAsync(Id);
        Claims = await UserManager.GetClaimsAsync(user);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync([Required] string task, [Required] string type,
        [Required] string value, string? oldValue)
    {
        var user = await UserManager.FindByIdAsync(Id);
        Claims = await UserManager.GetClaimsAsync(user);

        if (ModelState.IsValid)
        {
            var claim = new Claim(type, value);
            var result = IdentityResult.Success;

            switch (task)
            {
                case "add":
                    result = await UserManager.AddClaimAsync(user, claim);
                    break;
                case "change":
                    result = await UserManager.ReplaceClaimAsync(user, new Claim(type, oldValue), claim);
                    break;
                case "delete":
                    result = await UserManager.RemoveClaimAsync(user, claim);
                    break;
            }

            if (result.Process(ModelState))
            {
                return RedirectToPage();
            }
        }

        return Page();
    }
}