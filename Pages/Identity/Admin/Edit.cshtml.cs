using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Pages.Identity.Admin;

public class EditModel : AdminPageModel
{
    public UserManager<IdentityUser> UserManager { get; set; }
    public IdentityUser? IdentityUser { get; set; }

    [BindProperty(SupportsGet = true)] public string? Id { get; set; }

    public EditModel(UserManager<IdentityUser> mgr)
    {
        UserManager = mgr;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (string.IsNullOrEmpty(Id))
        {
            return RedirectToPage("SelectUser", new { Label = "Edit User", Callback = "Edit" });
        }

        IdentityUser = await UserManager.FindByIdAsync(Id);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync([FromForm(Name = "IdentityUser")] EditBindingTarget userData)
    {
        if (!string.IsNullOrEmpty(Id) && ModelState.IsValid)
        {
            var user = await UserManager.FindByIdAsync(Id);
            if (user != null)
            {
                user.UserName = userData.Username;
                user.Email = userData.Email;
                user.EmailConfirmed = true;
                if (!string.IsNullOrEmpty(userData.PhoneNumber))
                {
                    user.PhoneNumber = userData.PhoneNumber;
                }
            }
            var result = await UserManager.UpdateAsync(user);
            if (result.Process(ModelState))
            {
                return RedirectToPage();
            }
        }

        IdentityUser = await UserManager.FindByIdAsync(Id);
        return Page();
    }
}