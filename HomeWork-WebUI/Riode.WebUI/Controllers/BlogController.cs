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
        public IActionResult Index()
        {

            var blogs = db.Blogs
               .Include(p => p.Images)
               .Where(c => c.DeleteByUserId == null)
               .ToList();


            return View(blogs);
        }

        //Blog Details+
        public IActionResult Details(int id)
        {

            var blogdetails = db.Blogs
                 .Include(b => b.Images)
                 .FirstOrDefault(b => b.Id == id && b.DeleteByUserId == null);


            if (blogdetails == null)
            {
                return NotFound();
            }

            return View(blogdetails);
        }
    }
}
