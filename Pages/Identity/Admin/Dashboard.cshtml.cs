using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity.Admin;

public class DashboardModel : AdminPageModel
{
    public UserManager<IdentityUser> UserManager { get; set; }

    

    public int UsersCount { get; set; } = 0;
    public int UsersUnconfirmed { get; set; } = 0;
    public int UsersLockedout { get; set; } = 0;
    public int UsersTwoFactor { get; set; } = 0;

    private readonly string[] emails = { "alice@example.com", "bob@example.com", "charlie@example.com" };

    public string DashboardRole { get; set; }
    public DashboardModel(UserManager<IdentityUser> mng, IConfiguration cfg)
    {
        UserManager = mng;
        DashboardRole = cfg["Dashboard:Role"] ?? "Dashboard";
    }

    public void OnGet()
    {
        UsersCount = UserManager.Users.Count();
        UsersUnconfirmed= UserManager.Users.Count(u => !u.EmailConfirmed);
        UsersLockedout = UserManager.Users.Count(u => u.LockoutEnabled && u.LockoutEnd > DateTimeOffset.Now);
        UsersTwoFactor = UserManager.Users.Count(u => u.TwoFactorEnabled);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        foreach (var user in UserManager.Users.ToList())
        {
            if (emails.Contains(user.Email) || !await UserManager.IsInRoleAsync(user, DashboardRole))
            {
                var result = await UserManager.DeleteAsync(user);
                result.Process(ModelState);
            }
            
        }
        foreach (var email in emails)
        {
            var userObject = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            var result = await UserManager.CreateAsync(userObject);
            if (result.Process(ModelState))
            {
                result = await UserManager.AddPasswordAsync(userObject, "mysecret");
                result.Process(ModelState);
            }
            result.Process(ModelState);
        }

        if (ModelState.IsValid)
        {
            return RedirectToPage();
        }

        return Page();
    }
}