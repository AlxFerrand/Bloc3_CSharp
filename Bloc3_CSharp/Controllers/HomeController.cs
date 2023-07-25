using Bloc3_CSharp.Data;
using Bloc3_CSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Bloc3_CSharp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Catalog()
        {
            List<Product> products = _context.Products.Include(p => p.Category).ToList();
            List<Category> categories = _context.Categories.ToList();
            List<Articles> articles = new List<Articles>();
            foreach (var p in products)
            {
                articles.Add(new Articles(p, _context));
            }
            CatalogViewModel vm = new CatalogViewModel(articles,categories);

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}