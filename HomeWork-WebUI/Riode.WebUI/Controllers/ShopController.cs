using Microsoft.AspNetCore.Mvc;
using Riode.WebUI.Migrations;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Controllers
{
    public class ShopController : Controller
    {

        // index
        public IActionResult Index()
        {
            ShopIndexViewModel vm = new ShopIndexViewModel();

            RiodeDbContext db = new RiodeDbContext();

            vm.brands = db.Brands
                .Where(b => b.DeleteByUserId == null)
                .ToList();

            vm.productSizes = db.ProductSizes
                 .Where(b => b.DeleteByUserId == null)
                 .ToList();

            vm.productColors = db.ProductColors
                 .Where(b => b.DeleteByUserId == null)
                 .ToList();

            return View(vm);
        }
        // etrafli
        public IActionResult Details()
        {
            return View();
        }
    }
}
