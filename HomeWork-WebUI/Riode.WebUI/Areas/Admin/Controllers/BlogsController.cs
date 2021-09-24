using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [AllowAnonymous]

    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly RiodeDbContext db;
        private readonly IWebHostEnvironment env;

        public BlogsController(RiodeDbContext db,IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }

        // GET: Admin/Blogs
        public async Task<IActionResult> Index()
        {
            ViewBag.Count = db.Blogs.Where(b=>b.DeleteByUserId==null).Count();
            return View(await db.Blogs.Where(b=>b.DeleteByUserId==null).ToListAsync());
        }

        // GET: Admin/Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await db.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Admin/Blogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog, IFormFile file)
        {

            if (file == null)
            {
                ModelState.AddModelError("file", "sekil secilmeyib");
            }

            if (ModelState.IsValid)
            {
                string extension = Path.GetExtension(file.FileName);  //.jpg tapmaq ucundur. png .gng 

                blog.ImagePati= $"{Guid.NewGuid()}{extension}";//imagenin name 


                string phsicalFileName = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", "blog", "mask", blog.ImagePati);

                using (var stream=new FileStream(phsicalFileName,FileMode.Create,FileAccess.Write))
                {
                    await file.CopyToAsync(stream);
                }

                db.Add(blog);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await db.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Blog blog,IFormFile file ,string fileTemp)
        {


            if (id != blog.Id)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(fileTemp) && file == null)
            {
                ModelState.AddModelError("file", "sekil secilmeyib");
            }

        
            if (ModelState.IsValid)
            {
                try
                {
                    //db.Update(blog);

                    var entity = await db.Blogs.FirstOrDefaultAsync(b => b.Id == id && b.DeleteByUserId == null);

                    entity.Title = blog.Title;
                    entity.Body = blog.Body;

                    
                    if (file != null)
                    {

                        string extension = Path.GetExtension(file.FileName);  //.jpg tapmaq ucundur.

                        blog.ImagePati = $"{Guid.NewGuid()}{extension}";//imagenin name 


                        string phsicalFileName = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", "blog", "mask", blog.ImagePati);

                        using (var stream = new FileStream(phsicalFileName, FileMode.Create, FileAccess.Write))
                        {
                            await file.CopyToAsync(stream);
                        }

                        if (!string.IsNullOrWhiteSpace(entity.ImagePati))
                        {
                            System.IO.File.Delete(Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", "blog", "mask", entity.ImagePati));

                        }
                        entity.ImagePati = blog.ImagePati;
                    }

                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
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
            return View(blog);
        }

        // GET: Admin/Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await db.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Admin/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await db.Blogs.FindAsync(id);
            blog.DeleteData = DateTime.Now;
            blog.DeleteByUserId = 1;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

          
        }

        private bool BlogExists(int id)
        {
            return db.Blogs.Any(e => e.Id == id);
        }
    }
}
