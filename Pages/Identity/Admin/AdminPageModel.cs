using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Identity.Admin;

[Authorize(Roles = "Dashboard")]
public class AdminPageModel : PageModel
{
}