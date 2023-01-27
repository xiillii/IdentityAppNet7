using Microsoft.AspNetCore.Identity;

namespace IdentityApp.Pages.Identity.Admin;

public class FeaturesModel : AdminPageModel
{
    public UserManager<IdentityUser> UserManager { get; set; }
    public IEnumerable<(string, string)> Features { get; set; }

    public FeaturesModel(UserManager<IdentityUser> mgr)
    {
        UserManager = mgr;
    }

    public void OnGet()
    {
        Features = UserManager.GetType().GetProperties().Where(p => p.Name.StartsWith("Supports")).OrderBy(p => p.Name)
            .Select(p => (p.Name, p.GetValue(UserManager).ToString()));
    }
}