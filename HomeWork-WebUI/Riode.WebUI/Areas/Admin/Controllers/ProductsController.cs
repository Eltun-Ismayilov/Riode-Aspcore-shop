using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using Riode.WebUI.Model.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly RiodeDbContext db;
        private readonly IWebHostEnvironment env;

        public ProductsController(RiodeDbContext db,IWebHostEnvironment env)
        {
           this.db = db;
           this.env = env;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var riodeDbContext = db.products
                .Include(p=>p.Images)
                .Include(p => p.Brands);
            return View(await riodeDbContext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.products
                .Include(p => p.Brands)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["BrandsId"] = new SelectList(db.Brands, "Id", "Id");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Sku,BrandsId,ShopDescription,Description,Id,CreateByUserId,CreateData,DeleteByUserId,DeleteData")] Product product, ImageItemFormModel[] images)
        {
            if (images==null || !images.Any(i=>i.File !=null))
            {
                ModelState.AddModelError("Images", "Sekil Secilmeyib");
            }

            if (ModelState.IsValid)
            {
                product.Images = new List<ProductImage>();
                foreach (var image in images.Where(i=>i.File !=null))
                {
                    string extension = Path.GetExtension(image.File.FileName);
                    string imagepath = $"{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid()}{extension}";
                    string phy = Path.Combine(env.ContentRootPath,
                        "wwwroot",
                        "uploads",
                        "images",
                        imagepath);

                    using (var stream=new FileStream(phy,FileMode.Create,FileAccess.Write))
                    {
                        await image.File.CopyToAsync(stream);
                    }
                    product.Images.Add(new ProductImage
                    {
                        IsMain=image.IsMain,
                        FileName= imagepath
                    });
                }

                db.products.Add(product);
                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["BrandsId"] = new SelectList(db.Brands, "Id", "Id", product.BrandsId);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandsId"] = new SelectList(db.Brands, "Id", "Id", product.BrandsId);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Sku,BrandsId,ShopDescription,Description,Id,CreateByUserId,CreateData,DeleteByUserId,DeleteData")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(product);
                    await db.SaveChangesAsync();
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
            ViewData["BrandsId"] = new SelectList(db.Brands, "Id", "Id", product.BrandsId);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.products
                .Include(p => p.Brands)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await db.products.FindAsync(id);
            db.products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return db.products.Any(e => e.Id == id);
        }
    }
}
