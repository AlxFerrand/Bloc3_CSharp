using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bloc3_CSharp.Data;
using Bloc3_CSharp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Bloc3_CSharp.Services.abstractServices;
using Microsoft.AspNetCore.WebUtilities;
using Bloc3_CSharp.Services.concretServices;
using Microsoft.Data.SqlClient;

namespace Bloc3_CSharp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICreateArticleService _createArticleService;
        private readonly ISaveFilesService _saveFilesService;

        public ProductsController(ApplicationDbContext context, ICreateArticleService createArticleService, ISaveFilesService saveFilesService)
        {
            _context = context;
            _createArticleService = createArticleService;
            _saveFilesService = saveFilesService;
        }

        // GET: Products
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index(int? catId, string sortOrder)
        {
            //Recuperation des produit en fonction de la category
            ViewData["order"] = String.IsNullOrEmpty(sortOrder) ? "id" : sortOrder;
            List<Product> products = new List<Product>();
            if (catId == 0 || catId == null)
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
            switch (sortOrder)
            {
                case "id":
                    articles = articles.OrderBy(d => d.Id).ToList();
                    break;
                case "Label":
                    articles = articles.OrderBy(d => d.Label).ToList();
                    break;
                case "Label_desc":
                    articles = articles.OrderByDescending(d => d.Label).ToList();
                    break;
                case "BasePrice":
                    articles = articles.OrderBy(d => d.BasePrice).ToList();
                    break;
                case "BasePrice_desc":
                    articles = articles.OrderByDescending(d => d.BasePrice).ToList();
                    break;
                case "CategoryName":
                    articles = articles.OrderBy(d => d.CategoryName).ToList();
                    break;
                case "CategoryName_desc":
                    articles = articles.OrderByDescending(d => d.CategoryName).ToList();
                    break;
                case "DiscountId":
                    articles = articles.OrderBy(d => d.DiscountId).ToList();
                    break;
                case "DiscountId_desc":
                    articles = articles.OrderByDescending(d => d.DiscountId).ToList();
                    break;
                default:
                    articles = articles.OrderBy(d => d.Id).ToList();
                    break;
            }

            ViewData["CatId"] = new SelectList(_context.Categories, "Id", "Name", catId);
            CatalogViewModel vm = new CatalogViewModel(articles, categories);
            return View(vm);
        }

        // GET: Products/Details/5 (Desactivé)
        /*[Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }*/

        // GET: Products/Create
        [Authorize(Roles = "SuperAdmin, Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Label,Description,Price,CategoryId,PictureName")] Product product)
        {
            if (product.Price < 0)
            {
                ModelState.AddModelError("Price", "Price must be greater than 0");
            }
            if (_context.Categories.Find(product.CategoryId) == null)
            {
                ModelState.AddModelError("CategoryId", "Category not existe ");
            }
            string saveResult = "";
            try
            {
                saveResult = _saveFilesService.SaveFileToImgDirectory(Request.Form.Files[0], product.Label + "_");
            }catch (Exception e)
            {
                saveResult = "";
            }
            if (saveResult.Contains("Error :"))
            {
                ModelState.AddModelError("PictureName", saveResult);
            }            
            if (ModelState.IsValid)
            {
                product.PictureName = saveResult;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Label,Description,Price,CategoryId,DiscountId,PictureName")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            if (product.Price < 0)
            {
                ModelState.AddModelError("Price", "Price must be greater than 0");
            }
            if (_context.Categories.Find(product.CategoryId) == null)
            {
                ModelState.AddModelError("CategoryId", "Category not existe ");
            }
            string saveResult = "";
            string oldPicture = "";
            try
            {
                saveResult = _saveFilesService.SaveFileToImgDirectory(Request.Form.Files[0], product.Label + "_");
            }
            catch (Exception e)
            {
                saveResult = product.PictureName;
            }
            if (saveResult.Contains("Error :"))
            {
                ModelState.AddModelError("PictureName", saveResult);
            }
            if (!saveResult.Equals(product.PictureName))
            {
                oldPicture = product.PictureName;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    product.PictureName = saveResult;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    _saveFilesService.DeleteFileToImgDirectory(oldPicture);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            _saveFilesService.DeleteFileToImgDirectory(product.PictureName);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
