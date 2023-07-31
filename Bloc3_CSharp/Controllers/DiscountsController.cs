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

namespace Bloc3_CSharp.Controllers
{
    public class DiscountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICreateArticleService _createArticleService;

        public DiscountsController(ApplicationDbContext context, ICreateArticleService createArticleService)
        {
            _context = context;
            _createArticleService = createArticleService;
        }

        // GET: Discounts
        [Authorize(Roles = "SuperAdmin, Admin")]
        public IActionResult Index(string sortOrder)
        {
            ViewData["order"] = String.IsNullOrEmpty(sortOrder) ? "id" : sortOrder;
            List<Discount> discounts = new List<Discount>();
            switch (sortOrder)
            {
                case "id":
                    discounts = _context.Discounts.OrderBy(d => d.Id).ToList();
                    break;
                case "onDate":
                    discounts = _context.Discounts.OrderBy(d => d.OnDate).ToList();
                    break;
                case "onDate_desc":
                    discounts = _context.Discounts.OrderByDescending(d => d.OnDate).ToList();
                    break;
                case "offDate":
                    discounts = _context.Discounts.OrderBy(d => d.OffDate).ToList();
                    break;
                case "offDate_desc":
                    discounts = _context.Discounts.OrderByDescending(d => d.OffDate).ToList();
                    break;
                case "value":
                    discounts = _context.Discounts.OrderBy(d => d.Value).ToList();
                    break;
                case "value_desc":
                    discounts = _context.Discounts.OrderByDescending(d => d.Value).ToList();
                    break;
                default:
                    discounts = _context.Discounts.OrderBy(d => d.Id).ToList();
                    break;
            }
            Dictionary<int,int> nomberOfProducts = new Dictionary<int,int>();
            foreach (var discount in discounts)
            {
                nomberOfProducts.Add(discount.Id, _context.Products.Include(p => p.Category).Where(p => p.DiscountId == discount.Id).Count());
            }
            DiscountsIndexViewModel vm = new DiscountsIndexViewModel(discounts,nomberOfProducts);
            return View(vm);
        }


        // GET: Discounts/Details/5 (Desactivé)
        /*[Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Discounts == null)
            {
                return NotFound();
            }

            var discount = await _context.Discounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }*/

        // GET: Discounts/Create
        [Authorize(Roles = "SuperAdmin, Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Discounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OnDate,OffDate,Value")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(discount);
        }

        // GET: Discounts/Edit/5
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Discounts == null)
            {
                return NotFound();
            }

            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }
            return View(discount);
        }

        // POST: Discounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OnDate,OffDate,Value")] Discount discount)
        {
            if (id != discount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountExists(discount.Id))
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
            return View(discount);
        }

        // GET: Discounts/Delete/5
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Discounts == null)
            {
                return NotFound();
            }

            var discount = await _context.Discounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discount == null)
            {
                return NotFound();
            }

            return View(discount);
        }

        // POST: Discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Discounts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Discounts'  is null.");
            }
            var discount = await _context.Discounts.FindAsync(id);
            if (discount != null)
            {
                _context.Discounts.Remove(discount);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Discounts/GetAffectedProducts/5
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> GetAffectedProducts (int id)
        {
            if (id == 0 || _context.Discounts == null)
            {
                return NotFound();
            }

            var discount = await _context.Discounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discount == null)
            {
                return NotFound();
            }
            var productsListOfDiscount = _context.Products.Include(p => p.Category).Where(p => p.DiscountId == id).ToList();
            List<Articles> articlesListOfDiscount = new List<Articles>();
            foreach (var p in productsListOfDiscount)
            {
                articlesListOfDiscount.Add(_createArticleService.CreateArticle(p, _context));
            }

            AffectedProductsViewModel vm = new AffectedProductsViewModel(discount, articlesListOfDiscount);

            return View(vm);
        }

        // POST: Discounts/GetAffectedProducts/5
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetAffectedProducts(int DiscountId,int[] DeleteIds)
        {
            foreach (var id in DeleteIds)
            {
                Product product = _context.Products.Find(id);
                if (product.DiscountId == DiscountId)
                {
                    product.DiscountId = 0;
                    try
                    {
                        _context.Update(product);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return NotFound();
                    }
                }       
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Discounts/AddAffectedProducts/5
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> AddAffectedProducts(int id)
        {
            if (id == 0 || _context.Discounts == null)
            {
                return NotFound();
            }

            var discount = await _context.Discounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discount == null)
            {
                return NotFound();
            }
            var productsListNotOnDiscount = _context.Products.Include(p => p.Category).Where(p => p.DiscountId != id).ToList();
            List<Articles> articlesListNotOnDiscount = new List<Articles>();
            foreach (var p in productsListNotOnDiscount)
            {
                articlesListNotOnDiscount.Add(_createArticleService.CreateArticle(p, _context));
            }

            AffectedProductsViewModel vm = new AffectedProductsViewModel(discount, articlesListNotOnDiscount);

            return View(vm);
        }

        // POST: Discounts/AddAffectedProducts/int[]
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAffectedProducts(int DiscountId, int[] AddIds)
        {
            foreach (var id in AddIds)
            {
                Product product = _context.Products.Find(id);
                product.DiscountId = DiscountId;
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }

            }
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountExists(int id)
        {
          return (_context.Discounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
