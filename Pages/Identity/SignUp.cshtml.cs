using IdentityApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace IdentityApp.Pages.Identity;

[AllowAnonymous]
public class SignUpModel : UserPageModel
{
    public UserManager<IdentityUser> UserManager { get; set; }
    public IdentityEmailService EmailService { get; set; }
    public SignInManager<IdentityUser> SignInManager { get; set; }

    [BindProperty]
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [BindProperty]
    [Required]
    public string Password { get; set; }

    [BindProperty]
    [Required]
    [Compare(nameof(Password))]
    public string? ConfirmPassword { get; set; }

    public IEnumerable<AuthenticationScheme> ExternalSchemes { get; set; }

    public SignUpModel(UserManager<IdentityUser> userManager, IdentityEmailService emailService, SignInManager<IdentityUser> signMgr)
    {
        UserManager = userManager;
        EmailService = emailService;     
        SignInManager = signMgr;
    }

    public async Task OnGetAsync() => ExternalSchemes = await SignInManager.GetExternalAuthenticationSchemesAsync();

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = await UserManager.FindByEmailAsync(Email);
            if (user != null && !await UserManager.IsEmailConfirmedAsync(user))
            {
                return RedirectToPage("SignUpConfirm");
            }
            user = new IdentityUser
            {
                UserName = Email,
                Email = Email,
            };
            var result = await UserManager.CreateAsync(user);
            if (result.Process(ModelState))
            {
                result = await UserManager.AddPasswordAsync(user, Password);
                if (result.Process(ModelState))
                {
                    await EmailService.SendAccountConfirmEmail(user, "SignUpConfirm");
                    return RedirectToPage("SignUpConfirm");
                }
                await UserManager.DeleteAsync(user);
            }
        }

        return Page();
    }
}
