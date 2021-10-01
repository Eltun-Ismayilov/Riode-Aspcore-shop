using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Appcode.Application.OneCategoryModelu;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [AllowAnonymous]

    [Area("Admin")]
    public class OneCategoriesController : Controller
    {
        private readonly RiodeDbContext db;
        private readonly IMediator mediator;

        public OneCategoriesController(RiodeDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        [Authorize(Policy = "admin.OneCategory.Index")]
        public async Task<IActionResult> Index()
        {


            ViewBag.Count = db.OneCategories.Count();
            return View(await db.OneCategories.Where(c => c.DeleteByUserId == null).ToListAsync());
        }
        [Authorize(Policy = "admin.OneCategory.Details")]
        public async Task<IActionResult> Details(CategorySingleQuery query)
        {
            var respons = await mediator.Send(query);

            if (respons == null)
            {
                return NotFound();
            }

            return View(respons);
        }

        [Authorize(Policy = "admin.OneCategory.Create")]

        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(db.OneCategories, "Id", "Name");
            return View();
        }



        [Authorize(Policy = "admin.OneCategory.Create")]

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
            return View(await db.OneCategories.Where(o => o.DeleteData == null).ToListAsync());
        }

        [Authorize(Policy = "admin.OneCategory.Edit")]
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

        [Authorize(Policy = "admin.OneCategory.Edit")]

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

        [Authorize(Policy = "admin.OneCategory.Delete")]
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

        [Authorize(Policy = "admin.OneCategory.Delete")]
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
