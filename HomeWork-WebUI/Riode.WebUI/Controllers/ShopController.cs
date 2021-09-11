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

        // Shop index+
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


            vm.Products = db.products
                .Include(p => p.Images.Where(i => i.IsMain == true))
                .Include(c => c.Brands)
                .Where(c => c.DeleteByUserId == null)
                .ToList();


            vm.OneCategories = db.OneCategories
            .Include(c => c.Parent)                                // children chilren getiri yeni incude include 
            .Include(c => c.Children)                              // her bir category children apar
            .ThenInclude(c => c.Children)                          // children chilren getiri yeni incude include 
            .ThenInclude(c => c.Children)
            .Where(c => c.ParentId == null && c.DeleteByUserId == null)
            .ToList();


            return View(vm);
        }
        // Shop details+ 
        public IActionResult Details(int id)
        {

            var product = db.products
                .Include(i => i.Images)
                .Include(i => i.Brands)
                .Include(i => i.SpecificationValues.Where(s => s.DeleteByUserId == null))
                .ThenInclude(i => i.Specification)
                .FirstOrDefault(p => p.Id == id && p.DeleteByUserId == null);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        [HttpPost]
        public IActionResult Filter(ShopFilterFormModel model)
        {
            var query = db.products
                .Include(p => p.Images.Where(i => i.IsMain == true))
                .Include(c => c.Brands)
                .Include(c => c.ProductSizeColorCollection)
                .Where(c => c.DeleteByUserId == null)
                .AsQueryable();



            if (model?.brands?.Count() > 0)
            {
                query = query.Where(p => model.brands.Contains(p.BrandsId)); //nie bu cur axralilir?
            }
            if (model?.sizes?.Count() > 0)
            {
                query = query.Where(p => p.ProductSizeColorCollection.Any(pscc => model.sizes.Contains(pscc.SizeId)));
            }
            if (model?.colors?.Count() > 0)
            {
                query = query.Where(p => p.ProductSizeColorCollection.Any(pscc => model.colors.Contains(pscc.ColorId)));
            }

            return PartialView("_ProdactContainer", query.ToList());

        }
    }
}
