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
    public class QuestionsController : Controller
    {
        private readonly RiodeDbContext db;

        public QuestionsController(RiodeDbContext db)
        {
            this.db = db;
        }

        [Authorize(Policy = "admin.Question.Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Count = db.Questions.Count();
            return View(await db.Questions.Where(q=>q.DeleteByUserId==null).ToListAsync());
           
        }

        [Authorize(Policy = "admin.Question.Details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questions = await db.Questions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questions == null)
            {
                return NotFound();
            }

            return View(questions);
        }

        [Authorize(Policy = "admin.Question.Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "admin.Question.Create")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Question,Answer,Id,CreateByUserId,CreateData,DeleteByUserId,DeleteData")] Questions questions)
        {
            if (ModelState.IsValid)
            {
                db.Add(questions);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(questions);
        }

        [Authorize(Policy = "admin.Question.Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questions = await db.Questions.FindAsync(id);
            if (questions == null)
            {
                return NotFound();
            }
            return View(questions);
        }

        [Authorize(Policy = "admin.Question.Edit")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Question,Answer,Id,CreateByUserId,CreateData,DeleteByUserId,DeleteData")] Questions questions)
        {
            if (id != questions.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(questions);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionsExists(questions.Id))
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
            return View(questions);
        }

        [Authorize(Policy = "admin.Question.Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questions = await db.Questions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questions == null)
            {
                return NotFound();
            }

            return View(questions);
        }

        [Authorize(Policy = "admin.Question.Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questions = await db.Questions.FindAsync(id);
            questions.DeleteData = DateTime.Now;
            questions.DeleteByUserId = 1;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionsExists(int id)
        {
            return db.Questions.Any(e => e.Id == id);
        }
    }
}
