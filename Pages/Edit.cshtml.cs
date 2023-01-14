using IdentityApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages
{
    public class EditModel : PageModel
    {
        public ProductDbContext DbContext { get; set; }

        public Product Product { get; set; }

        public EditModel(ProductDbContext ctx)
        {
            DbContext = ctx;
        }

        public void OnGet(long id)
        {
            Product = DbContext.Find<Product>(id) ?? new Product();
        }

        public IActionResult OnPost([Bind(Prefix = "Product")] Product p)
        {
            DbContext.Update(p);
            DbContext.SaveChanges();

            return RedirectToPage("Admin");
        }

    }
}
