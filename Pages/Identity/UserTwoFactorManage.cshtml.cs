using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity;

public class UserTwoFactorManageModel : UserPageModel
{
    public UserManager<IdentityUser> UserManager { get; set; }
    public SignInManager<IdentityUser> SignInManager { get; set; }

    public IdentityUser IdentityUser { get; set; }

    public UserTwoFactorManageModel(UserManager<IdentityUser> usrMgr, SignInManager<IdentityUser> signMgr)
    {
        UserManager = usrMgr;
        SignInManager = signMgr;
    }

    public async Task<bool> IsTwoFactorEnabled() => await UserManager.GetTwoFactorEnabledAsync(IdentityUser);

    public async Task OnGetAsync() => IdentityUser = await UserManager.GetUserAsync(User);

    public async Task<IActionResult> OnPostDisable()
    {
        IdentityUser = await UserManager.GetUserAsync(User);
        var result = await UserManager.SetTwoFactorEnabledAsync(IdentityUser, false);
            
        if (result.Process(ModelState))
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", new { });
        }

        return Page();
    }

    public async Task<IActionResult> OnPostGenerateCodes()
    {
        IdentityUser = await UserManager.GetUserAsync(User);
        TempData["RecoveryCodes"] = await UserManager.GenerateNewTwoFactorRecoveryCodesAsync(IdentityUser, 10);

        return RedirectToPage("UserRecoveryCodes");
    }
}