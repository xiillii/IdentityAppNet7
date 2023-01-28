using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity
{
    public class IndexModel : UserPageModel
    {
        public UserManager<IdentityUser> UserManager { get; set; }

        public IndexModel(UserManager<IdentityUser> mgr)
        {
            UserManager = mgr;
        }

        public string Email { get; set; }
        public string Phone { get; set; }

        public async Task OnGetAsync()
        {
            var currentUser = await UserManager.GetUserAsync(User);
            Email = currentUser?.Email ?? "(No Value)";
            Phone = currentUser?.PhoneNumber ?? "(No Value)";
        }
    }
}
