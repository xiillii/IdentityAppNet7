using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity
{
    public class UserDeleteModel : UserPageModel
    {
        public UserManager<IdentityUser> UserManager{ get; set; }
        public SignInManager<IdentityUser> SignInManager { get; set; }

        public UserDeleteModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var idUser = await UserManager.GetUserAsync(User);
            var result = await UserManager.DeleteAsync(idUser);
            if (result.Process(ModelState))
            {
                await SignInManager.SignOutAsync();

                return Challenge();
            }

            return Page();
        }
    }
}
