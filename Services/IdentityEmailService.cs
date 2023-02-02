using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace IdentityApp.Services;

public class IdentityEmailService
{
    public IEmailSender EmailSender { get; set; }
    public UserManager<IdentityUser> UserManager { get; set; }
    public IHttpContextAccessor ContextAccessor { get; set; }
    public LinkGenerator LinkGenerator { get; set; }
    public TokenUrlEncoderService TokenEncoder { get; set; }

    public IdentityEmailService(IEmailSender sender, UserManager<IdentityUser> mgr, IHttpContextAccessor accessor, LinkGenerator generator, TokenUrlEncoderService encoder)
    {
        EmailSender = sender;
        UserManager = mgr;
        ContextAccessor = accessor;
        LinkGenerator = generator;
        TokenEncoder = encoder;
    }

    private string GetUrl(string emailAddress, string token, string page)
    {
        var safeToken = TokenEncoder.EncodeToken(token);

        return LinkGenerator.GetUriByPage(ContextAccessor.HttpContext, page, null,
            new { email = emailAddress, token = safeToken });
    }

    public async Task SendPasswordRecoveryEmail(IdentityUser user, string confirmationPage)
    {
        var token = await UserManager.GeneratePasswordResetTokenAsync(user);
        var url = GetUrl(user.Email, token, confirmationPage);
        await EmailSender.SendEmailAsync(user.Email, "Set Your Password",
            $"Please set your password by <a href={url}>clicking here</a>.");
    }

    public async Task SendAccountConfirmEmail(IdentityUser user, string confirmationPage)
    {
        var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
        var url = GetUrl(user.Email, token, confirmationPage);

        await EmailSender.SendEmailAsync(user.Email, "Complete Your Account Setup",
            $"Please set up your account by <a href={url}>clicking here</a>");
    }
}