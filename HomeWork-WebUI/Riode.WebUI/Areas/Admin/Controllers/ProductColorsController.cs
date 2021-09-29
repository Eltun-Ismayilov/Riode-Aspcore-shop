using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductColorsController : Controller
    {
        private readonly RiodeDbContext db;

        public ProductColorsController(RiodeDbContext db)
        {
           this.db = db;
        }

        [Authorize(Policy = "admin.ProductColor.Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Count = db.ProductColors.Count();
            return View(await db.ProductColors.Where(c=>c.DeleteByUserId==null).ToListAsync());
        }

        [Authorize(Policy = "admin.ProductColor.Details")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productColor = await db.ProductColors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productColor == null)
            {
                return NotFound();
            }

            return View(productColor);
        }

        [Authorize(Policy = "admin.ProductColor.Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "admin.ProductColor.Create")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,SkuCode,description,Id,CreateByUserId,CreateData,DeleteByUserId,DeleteData")] ProductColor productColor)
        {
            if (ModelState.IsValid)
            {
                db.Add(productColor);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productColor);
        }

        [Authorize(Policy = "admin.ProductColor.Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productColor = await db.ProductColors.FindAsync(id);
            if (productColor == null)
            {
                return NotFound();
            }
            return View(productColor);
        }


        [Authorize(Policy = "admin.ProductColor.Edit")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,SkuCode,description,Id,CreateByUserId,CreateData,DeleteByUserId,DeleteData")] ProductColor productColor)
        {
            if (id != productColor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(productColor);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductColorExists(productColor.Id))
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
            return View(productColor);
        }

        [Authorize(Policy = "admin.ProductColor.Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productColor = await db.ProductColors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productColor == null)
            {
                return NotFound();
            }

            return View(productColor);
        }

        [Authorize(Policy = "admin.ProductColor.Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productColor = await db.ProductColors.FindAsync(id);
            productColor.DeleteData = DateTime.Now;
            productColor.DeleteByUserId = 1;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductColorExists(int id)
        {
            return db.ProductColors.Any(e => e.Id == id);
        }
    }
}
