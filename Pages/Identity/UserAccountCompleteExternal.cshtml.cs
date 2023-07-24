using IdentityApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityApp.Pages.Identity;

public class UserAccountCompleteExternalModel : UserPageModel
{
    public UserManager<IdentityUser> UserManager { get; set; }
    public SignInManager<IdentityUser> SignInManager { get; set; }
    public TokenUrlEncoderService TokenUrlEncoder { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Email { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Token { get; set; }

    public IdentityUser? IdentityUser { get; set; }

    public UserAccountCompleteExternalModel(UserManager<IdentityUser> usrMgr,
                                            SignInManager<IdentityUser> signMgr,
                                            TokenUrlEncoderService encoder)
    {
        UserManager = usrMgr;
        SignInManager = signMgr;
        TokenUrlEncoder = encoder;
    }

    public async Task<string> ExternalProvider() => (await UserManager.GetLoginsAsync(IdentityUser))
        .FirstOrDefault()?.ProviderDisplayName;

    public async Task<IActionResult> OnPostAsync(string provider)
    {
        IdentityUser = await UserManager.FindByEmailAsync(Email ?? "");
        var decodeToken = TokenUrlEncoder.DecodeToken(Token ?? "");
        var valid = await UserManager.VerifyUserTokenAsync(IdentityUser, UserManager.Options.Tokens.PasswordResetTokenProvider
            , UserManager<IdentityUser>.ResetPasswordTokenPurpose, decodeToken);

        if (!valid)
        {
            return Error("Invalid token");
        }

        var callbackUrl = Url.Page("UserAccountCompleteExternal"
            , "Callback", new { Email, Token });

        var props = SignInManager.ConfigureExternalAuthenticationProperties(provider, callbackUrl);


        return new ChallengeResult(provider, props);
    }

    public async Task<IActionResult> OnGetCallbackAsync()
    {
        var info = await SignInManager.GetExternalLoginInfoAsync();
        var email = info?.Principal?.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
        {
            return Error("External service has not provide an email address");
        } else if ((IdentityUser = await UserManager.FindByEmailAsync(email)) == null)
        {
            return Error("Your email address doesn't match");
        }

        var result = await UserManager.AddLoginAsync(IdentityUser, info);

        if (!result.Succeeded)
        {
            return Error("Cannot store external login");
        }
        return RedirectToPage(new { id = IdentityUser.Id });
    }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if ((id == null || (IdentityUser = await UserManager.FindByIdAsync(id)) == null) 
            && !TempData.ContainsKey("errorMessage"))
        {
            return RedirectToPage("SignIn");
        }
        return Page();
    }

    private IActionResult Error(string err)
    {
        TempData["errorMessage"] = err;

        return RedirectToPage();
    }
}
