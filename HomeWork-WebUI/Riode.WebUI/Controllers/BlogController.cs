using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Controllers
{
    public class BlogController : Controller
    {

        readonly RiodeDbContext db;
        public BlogController(RiodeDbContext db)
        {
            this.db = db;
        }
        //Blog Layout+
        public IActionResult Index(int page=1)
        {
            var productcount = 3;                                                                              
            ViewBag.PagesCaunt = decimal.Ceiling((decimal)db.Blogs.Where(d => d.DeleteByUserId == null).Count() / productcount);
            ViewBag.Page = page;    // 1




            var data = db.Blogs
                .Where(b=>b.PublishedDate!=null && b.DeleteByUserId==null)
                .Skip((page-1)*productcount).Take(productcount)
                .ToList();


            return View(data);
        }

        //Blog Details+
        public IActionResult Details(int id)
        {


            var data = db.Blogs
            .FirstOrDefault(b => b.PublishedDate != null && b.DeleteByUserId == null && b.Id == id);
          

            return View(data);
        }
    }
}
