using IdentityApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers;

public class StoreController : Controller
{
    private readonly ProductDbContext _context;

    public StoreController(ProductDbContext ctx)
    {
        _context = ctx;
    }

    public IActionResult Index()
    {
        return View(_context.Products);
    }
}