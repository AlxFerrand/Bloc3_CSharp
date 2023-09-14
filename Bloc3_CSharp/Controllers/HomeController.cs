using Bloc3_CSharp.Data;
using Bloc3_CSharp.Models;
using Bloc3_CSharp.Services.abstractServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
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

        public IActionResult Catalog(int catId,decimal MinPrice,decimal MaxPrice)
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
            List<Articles> articles = _createArticleService.CreateArticlesList(products);
            
            if (!(MinPrice < 0))
            {
                articles = articles.Where(a => a.Price > MinPrice).ToList();
            }
            else
            {
                MinPrice = 0;
            }
            if (!(MaxPrice <= 0))
            {
                articles = articles.Where(a => a.Price < MaxPrice).ToList();
            }
            else
            {
                MaxPrice = 0;
            }

            ViewData["MinPrice"] = MinPrice;
            ViewData["MaxPrice"] = MaxPrice;
            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name", catId);
            return View(articles);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}