using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Migrations;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity.FormModels;
using Riode.WebUI.Model.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Controllers
{
    public class ShopController : Controller
    {
        readonly RiodeDbContext db;
        public ShopController(RiodeDbContext db)
        {
            this.db = db;
        }

        // index
        public IActionResult Index()
        {
            ShopIndexViewModel vm = new ShopIndexViewModel();


            vm.brands = db.Brands
                .Where(b => b.DeleteByUserId == null)
                .ToList();

            vm.productSizes = db.ProductSizes
                 .Where(b => b.DeleteByUserId == null)
                 .ToList();

            vm.productColors = db.ProductColors
                 .Where(b => b.DeleteByUserId == null)
                 .ToList();

            vm.OneCategories = db.OneCategories
                .Include(c => c.Parent)                                // children chilren getiri yeni incude include 
                .Include(c => c.Children)                              // her bir category children apar
                .ThenInclude(c => c.Children)                          // children chilren getiri yeni incude include 
                .ThenInclude(c => c.Children)
                .Where(c => c.ParentId == null && c.DeleteByUserId == null)
                .ToList();

            vm.Products = db.products
                .Include(p=>p.Images.Where(i=>i.IsMain==true))
                .Include(c => c.Brands)
                .Where(c =>c.DeleteByUserId == null)
                .ToList();




            return View(vm);
        }
        // etrafli
        public IActionResult Details(int id)
        {

            var product = db.products
                .Include(i=>i.Images)
                .Include(i=>i.Brands)
                .FirstOrDefault(p => p.Id== id && p.DeleteByUserId==null);

            if (product==null)
            {
                return NotFound();
            }

            return View(product);
        }


        [HttpPost]
        public IActionResult Filter(ShopFilterFormModel model)
        {
            return Json(new { 
            
                error=false,
                data =model
            });
        }
    }
}
