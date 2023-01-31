using System.ComponentModel.DataAnnotations;
using IdentityApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity.Admin;

public class PasswordsModel : AdminPageModel
{
    public UserManager<IdentityUser> UserManager { get; set; }
    public IdentityEmailService EmailService { get; set; }
    public IdentityUser IdentityUser { get; set; }

    [BindProperty(SupportsGet = true)] public string Id { get; set; }

    [BindProperty]
    [Required]
    public string Password { get; set; }

    [BindProperty]
    [Compare(nameof(Password))]
    public string Confirmation { get; set; }


    public PasswordsModel(UserManager<IdentityUser> mng, IdentityEmailService service)
    {
        UserManager = mng;
        EmailService = service;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (string.IsNullOrEmpty(Id))
        {
            return RedirectToPage("SelectUser", new { Label = "Password", Callback = "Passwords" });
        }

        IdentityUser = await UserManager.FindByIdAsync(Id);
        return Page();
    }

    public async Task<IActionResult> OnPostSetPasswordAsync()
    {
        if (ModelState.IsValid)
        {
            IdentityUser = await UserManager.FindByIdAsync(Id);
            if (await UserManager.HasPasswordAsync(IdentityUser))
            {
                await UserManager.RemovePasswordAsync(IdentityUser);
            }

            var result = await UserManager.AddPasswordAsync(IdentityUser, Password);
            if (result.Process(ModelState))
            {
                TempData["message"] = "Password Changed";
                return RedirectToPage();
            }
        }

        return Page();
    }

    public async Task<IActionResult> OnPostUserChangeAsync()
    {
        IdentityUser = await UserManager.FindByIdAsync(Id);
        await UserManager.RemovePasswordAsync(IdentityUser);
        await EmailService.SendPasswordRecoveryEmail(IdentityUser, "/Identity/UserPasswordRecoveryConfirm");
        TempData["message"] = "Email Sent to User";
        return RedirectToPage();
    }
}