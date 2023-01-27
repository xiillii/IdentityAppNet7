using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity.Admin
{
    public class SelectUserModel : AdminPageModel
    {
        public UserManager<IdentityUser> UserManager { get; set; }
        public IEnumerable<IdentityUser>? Users { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Label { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Callback { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Filter { get; set; }

        public SelectUserModel(UserManager<IdentityUser> mgr) => UserManager = mgr;


        public void OnGet()
        {
            Users = UserManager.Users.Where(u => Filter == null || u.Email.Contains(Filter)).OrderBy(u => u.Email)
                .ToList();
        }

        public IActionResult OnPost() => RedirectToPage(new { Filter, Callback });
    }
}
