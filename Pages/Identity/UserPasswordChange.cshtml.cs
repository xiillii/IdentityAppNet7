using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity
{
    public class UserPasswordChangeModel : UserPageModel
    {
        public UserManager<IdentityUser> UserManager { get; set; }

        public UserPasswordChangeModel(UserManager<IdentityUser> mgr)
        {
            UserManager = mgr;
        }

        public async Task<IActionResult> OnPostAsync(PasswordChangeBindingTarget data)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.GetUserAsync(User);
                var result = await UserManager.ChangePasswordAsync(user, data.Current, data.NewPassword);

                if (result.Process(ModelState))
                {
                    TempData["message"] = "Password changed";
                    return RedirectToPage();
                }
            }

            return Page();
        }
    }
}
