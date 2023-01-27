using System.ComponentModel.DataAnnotations;

namespace IdentityApp.Pages.Identity.Admin;

public class EditBindingTarget
{
    [Required] public string Username { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Phone]
    public string? PhoneNumber { get; set; }
}