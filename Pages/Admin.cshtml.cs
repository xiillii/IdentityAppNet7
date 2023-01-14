using IdentityApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages;

public class AdminModel : PageModel
{
    public ProductDbContext DbContext { get; set; }

    public AdminModel(ProductDbContext ctx)
    {
        DbContext = ctx;
    }

    public IActionResult OnPost(long id)
    {
        var p = DbContext.Find<Product>(id);
        if (p != null)
        {
            DbContext.Remove(p);
            DbContext.SaveChanges();
        }

        return Page();
    }
}