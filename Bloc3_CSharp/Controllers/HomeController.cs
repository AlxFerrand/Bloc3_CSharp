﻿using Bloc3_CSharp.Data;
using Bloc3_CSharp.Models;
using Microsoft.AspNetCore.Authorization;
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
                articles.Add(new Articles(p, _context));
            }

            //
            Category selectedCategory;
            if (catId != null)
            {
                selectedCategory = _context.Categories.Find((int)catId);
            }
            else
            {
                selectedCategory = new Category();
            }
            

            CatalogViewModel vm = new CatalogViewModel(articles, categories, selectedCategory);

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}