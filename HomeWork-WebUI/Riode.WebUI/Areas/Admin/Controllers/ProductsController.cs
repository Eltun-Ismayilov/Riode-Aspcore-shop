using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]

    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly RiodeDbContext db;
        private readonly IWebHostEnvironment env;

        public ProductsController(RiodeDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var riodeDbContext = db.products
                .Include(p => p.Images)
                .Include(p => p.Brands)
                .Where(d => d.DeleteByUserId == null);

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
                .Include(p => p.Images)
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
            ViewData["BrandsId"] = new SelectList(db.Brands, "Id", "Name");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Bind("Name,Sku,BrandsId,ShopDescription,Description,Id,CreateByUserId,CreateData,DeleteByUserId,DeleteData")]
        public async Task<IActionResult> Create(Product product, ImageItemFormModel[] images)
        {
            if (images == null || !images.Any(i => i.File != null))
            {
                ModelState.AddModelError("Images", "Sekil Secilmeyib");
            }

            if (ModelState.IsValid)
            {
                product.Images = new List<ProductImage>();
                foreach (var image in images.Where(i => i.File != null))
                {
                    string extension = Path.GetExtension(image.File.FileName);
                    string imagepath = $"{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid()}{extension}";
                    string phy = Path.Combine(env.ContentRootPath,
                        "wwwroot",
                        "uploads",
                        "images",
                        imagepath);

                    using (var stream = new FileStream(phy, FileMode.Create, FileAccess.Write))
                    {
                        await image.File.CopyToAsync(stream);
                    }
                    product.Images.Add(new ProductImage
                    {
                        IsMain = image.IsMain,
                        FileName = imagepath
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

            var product = await db.products
                .Include(p => p.Images.Where(i => i.DeleteByUserId == null))
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandsId"] = new SelectList(db.Brands, "Id", "Id", product.BrandsId);
            return View(product);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var entity = await db.products
                    .Include(p=>p.Images.Where(pi=>pi.DeleteByUserId==null))
                    .FirstOrDefaultAsync(p => p.Id == id);

                entity.Name = product.Name;
                entity.Description = product.Description;



                //deleted

                int[] ids = product.Files
                    .Where(p => p.Id > 0 && string.IsNullOrWhiteSpace(p.TempPath))

                    .Select(p => p.Id.Value)
                    .ToArray();


                foreach (var item in ids)
                {
                    var oldimage = await db.ProductImages.FirstOrDefaultAsync(p => p.ProductId == entity.Id && p.Id == item);

                    if (oldimage == null)

                        continue; 


                    oldimage.DeleteData = DateTime.Now;
                    oldimage.DeleteByUserId = 1;
                }

                foreach (var item in product.Files.Where(f => (f.Id > 0 && !string.IsNullOrWhiteSpace(f.TempPath)) || (f.File != null && f.Id==null))) //Bazada olub deyisenler path+ id+
                {
                    if (item.File == null)
                    {
                        var oldimage = await db.ProductImages.FirstOrDefaultAsync(p => p.ProductId == entity.Id && p.Id == item.Id);

                        if (oldimage == null)

                            continue;

                        oldimage.IsMain = item.IsMain;
                    }
                    else if (item.File != null)
                    {
                        string extension = Path.GetExtension(item.File.FileName);  //.jpg tapmaq ucundur. png .gng 

                        string fileName = $"{Guid.NewGuid()}{extension}";//imagenin name 


                        string phsicalFileName = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", fileName);

                        using (var stream = new FileStream(phsicalFileName, FileMode.Create, FileAccess.Write))
                        {
                            await item.File.CopyToAsync(stream);
                        }

                        entity.Images.Add(new ProductImage
                        {
                            FileName=fileName,
                            IsMain=item.IsMain
                        });


                    }
                }

                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandsId"] = new SelectList(db.Brands, "Id", "Name", product.BrandsId);
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
                .Include(p => p.Images)
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
            product.DeleteData = DateTime.Now;
            product.DeleteByUserId = 1;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return db.products.Any(e => e.Id == id);
        }
    }
}
