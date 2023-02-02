using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity.Admin
{
    public class DeleteModel : AdminPageModel
    {
        public UserManager<IdentityUser> UserManager { get; set; }

        public IdentityUser? IdentityUser { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Id { get; set; }

        public DeleteModel(UserManager<IdentityUser> userManager)
        {
            UserManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToPage("SelectUser", new { Label = "Delete", Callback = "Delete" });
            }
            IdentityUser = await UserManager.FindByIdAsync(Id);            

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            IdentityUser = await UserManager.FindByIdAsync(Id);
            var result = await UserManager.DeleteAsync(IdentityUser);
            if (result.Process(ModelState))
            {
                return RedirectToPage("Dashboard");
            }

            return Page();
        }
    }
}
