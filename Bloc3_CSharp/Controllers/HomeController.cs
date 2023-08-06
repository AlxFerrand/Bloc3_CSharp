using Bloc3_CSharp.Data;
using Bloc3_CSharp.Models;
using Bloc3_CSharp.Services.abstractServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Bloc3_CSharp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ICreateArticleService _createArticleService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, ICreateArticleService createArticleService)
        {
            _logger = logger;
            _context = context;
            _createArticleService = createArticleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Catalog(int? catId)
        {
            //Recuperation des produit en fonction de la category
            List<Product> products = new List<Product>();
            if (catId == 0 || catId== null)
            {
                products = _context.Products.Include(p => p.Category).ToList();
            }
            else
            {
                products = _context.Products.Include(p => p.Category).Where(p => p.CategoryId == catId).ToList();
            }
            //Recuperation de la liste des categories
            List<Category> categories = _context.Categories.ToList();

            //Creation des Articles pour vm
            List<Articles> articles = new List<Articles>();
            foreach (var p in products)
            {
                articles.Add(_createArticleService.CreateArticle(p, _context));
            }

            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name", catId);
            CatalogViewModel vm = new CatalogViewModel(articles, categories);

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}