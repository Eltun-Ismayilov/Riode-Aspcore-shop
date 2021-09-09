using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using Riode.WebUI.Model.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Controllers
{
    public class HomeController : Controller
    {
        readonly RiodeDbContext db;
        public HomeController(RiodeDbContext db)
        {
            this.db = db;

        }

        // ana seyfe
        public IActionResult Index()
        {
            return View();
        }
        // contact
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactPost model)
        {

            if (ModelState.IsValid)
            {
                db.Add(model);
                db.SaveChanges();


                ModelState.Clear();

                return Json(new
                {
                    error = false,
                    message="Sorgunuz qeyde alindir"
                }); 


            }
            return Json(new
            {
                error = true,
                message = "Mellumatin dogrulugnu yoxluyun"
            });

        }
        // about
        public IActionResult AboutUS()
        {
            return View();
        }
        // Fak
        public IActionResult Faq()
        {
            ShopIndexViewModel vm = new ShopIndexViewModel();


           
            vm.Questions = db.Questions
               .Where(a => a.DeleteByUserId == null)
               .Include(a => a.Answers)
               .ToList();

            return View(vm);
        }
    }
}
