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
    [AllowAnonymous]

    [Area("Admin")]
    public class OneCategoriesController : Controller
    {
        private readonly RiodeDbContext db;

        public OneCategoriesController(RiodeDbContext db)
        {
            this.db = db;
        }

        // GET: Admin/OneCategories
        public async Task<IActionResult> Index()
        {
            var riodeDbContext = db.OneCategories;
       
            return View(await db.OneCategories.Where(o=>o.DeleteByUserId==null).ToListAsync());
        }
        // GET: Admin/OneCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oneCategory = await db.OneCategories
                .Include(o => o.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oneCategory == null)
            {
                return NotFound();
            }

            return View(oneCategory);
        }

        // GET: Admin/OneCategories/Create
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(db.OneCategories, "Id", "Name");
            return View();
        }

        // POST: Admin/OneCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OneCategory oneCategory)
        {
            if (ModelState.IsValid)
            {
                db.Add(oneCategory);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(db.OneCategories, "Id", "Name", oneCategory.ParentId);
            return View(oneCategory);
        }

        // GET: Admin/OneCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oneCategory = await db.OneCategories.FindAsync(id);
            if (oneCategory == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(db.OneCategories, "Id", "Name", oneCategory.ParentId);
            return View(oneCategory);
        }

        // POST: Admin/OneCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParentId,Name,Description,Id,CreateByUserId,CreateData,DeleteByUserId,DeleteData")] OneCategory oneCategory)
        {
            if (id != oneCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(oneCategory);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OneCategoryExists(oneCategory.Id))
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
            ViewData["ParentId"] = new SelectList(db.OneCategories, "Id", "Name", oneCategory.ParentId);
            return View(oneCategory);
        }

        // GET: Admin/OneCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oneCategory = await db.OneCategories
                .Include(o => o.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oneCategory == null)
            {
                return NotFound();
            }

            return View(oneCategory);
        }

        // POST: Admin/OneCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oneCategory = await db.OneCategories.FindAsync(id);
            oneCategory.DeleteData = DateTime.Now;
            oneCategory.DeleteByUserId = 1;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OneCategoryExists(int id)
        {
            return db.OneCategories.Any(e => e.Id == id);
        }
    }
}
