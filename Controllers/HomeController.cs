using IdentityApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductDbContext _context;

        public HomeController(ProductDbContext ctx) => _context = ctx;

        public IActionResult Index()
        {
            return View(_context.Products);
        }
    }
}
