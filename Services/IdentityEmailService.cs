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
}