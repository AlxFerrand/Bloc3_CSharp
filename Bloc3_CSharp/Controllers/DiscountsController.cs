﻿using System;
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

namespace Bloc3_CSharp.Controllers
{
    public class DiscountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiscountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Discounts
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index(string sortOrder)
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
            DiscountsIndexViewModel vm = new DiscountsIndexViewModel(discounts);
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

        private bool DiscountExists(int id)
        {
          return (_context.Discounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
