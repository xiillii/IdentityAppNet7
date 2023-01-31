using System.ComponentModel.DataAnnotations;
using IdentityApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity.Admin;

public class CreateModel : PageModel
{
    public UserManager<IdentityUser> UserManager { get; set; }
    public IdentityEmailService EmailService { get; set; }

    [BindProperty(SupportsGet = true)]
    [EmailAddress]
    public string? Email { get; set; }

    public CreateModel(UserManager<IdentityUser> mgr, IdentityEmailService service)
    {
        UserManager = mgr;
        EmailService = service;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser
            {
                UserName = Email,
                Email = Email,
                EmailConfirmed = true
            };
            var result = await UserManager.CreateAsync(user);
            if (result.Process(ModelState))
            {
                await EmailService.SendPasswordRecoveryEmail(user, "/Identity/UserAccountComplete");
                TempData["message"] = "Account Created";
                return RedirectToPage();
            }
        }

        return Page();
    }
}