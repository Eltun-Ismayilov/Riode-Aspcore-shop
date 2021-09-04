using Microsoft.AspNetCore.Mvc;
using Riode.WebUI.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Controllers
{
    public class HomeController : Controller
    {

      
        // ana seyfe
        public IActionResult Index()
        {
            return View();
        }
        // contact
        public IActionResult Contect()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contect(Contect contact)
        {
            return View();
        }
        // about
        public IActionResult AboutUS()
        {
            return View();
        }
        // Fak
        public IActionResult Faq()
        {
            return View();
        }
    }
}
