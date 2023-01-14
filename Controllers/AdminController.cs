using IdentityApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProductDbContext _context;

        public AdminController(ProductDbContext ctx)
        {
            _context = ctx;
        }

        public IActionResult Index()
        {
            return View(_context.Products);
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            var p = _context.Find<Product>(id);

            if (p != null)
            {
                return View("Edit", p);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Save(Product p)
        {
            _context.Update(p);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var p = _context.Find<Product>(id);

            if (p != null)
            {
                _context.Remove(p);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
