using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity.Admin;

public class LockoutsModel : AdminPageModel
{
    public UserManager<IdentityUser> UserManager{ get; set; }

    public IEnumerable<IdentityUser>? LockedOutUsers { get; set; }
    public IEnumerable<IdentityUser>? OtherUsers { get; set; }

    public LockoutsModel(UserManager<IdentityUser> userManager)
    {
        UserManager = userManager;
    }

    public async Task<TimeSpan> TimeLeft(IdentityUser user) 
        => (await UserManager.GetLockoutEndDateAsync(user)).GetValueOrDefault().Subtract(DateTimeOffset.Now);

    public void OnGet()
    {
        LockedOutUsers = UserManager.Users.Where(u => u.LockoutEnd.HasValue && u.LockoutEnd.Value > DateTimeOffset.Now)
            .OrderBy(u => u.Email).ToList();
        OtherUsers = UserManager.Users.Where(u => !u.LockoutEnd.HasValue || u.LockoutEnd.Value <= DateTimeOffset.Now)
            .OrderBy(u => u.Email).ToList();
    }

    public async Task<IActionResult> OnPostLockAsync(string id)
    {
        var user = await UserManager.FindByIdAsync(id);
        await UserManager.SetLockoutEnabledAsync(user, true);
        await UserManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddDays(5));

        await UserManager.UpdateSecurityStampAsync(user);

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUnlockAsync(string id)
    {
        var user = await UserManager.FindByIdAsync(id);
        await UserManager.SetLockoutEndDateAsync(user, null);

        return RedirectToPage();
    }
}
