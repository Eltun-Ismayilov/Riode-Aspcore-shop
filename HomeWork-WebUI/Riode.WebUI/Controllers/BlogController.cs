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
            var productcount = 2;                                                // 3                               // 2
            ViewBag.PagesCaunt = decimal.Ceiling((decimal)db.Blogs.Where(d => d.DeleteByUserId == null).Count() / productcount);
            ViewBag.Page = page;    // 1





            var blogs = db.Blogs
               .Include(p => p.Images)
               .Where(c => c.DeleteByUserId == null)
               .Skip((page-1) * productcount).Take(productcount)
               .ToList();


            return View(blogs);
        }

        //Blog Details+
        public IActionResult Details(int id)
        {
            CategoryViewsmodel vn = new CategoryViewsmodel();



            vn.blogCategories = db.BlogCategories
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .ThenInclude(c => c.Children)
                .Where(c => c.ParentId == null && c.DeleteByUserId == null) // cut gelirse model
                .ToList();



            vn.blog = db.Blogs
                .Include(p => p.Images)
                .FirstOrDefault(b => b.Id == id && b.DeleteByUserId == null); // tek gelirse model
          


            return View(vn);
        }
    }
}
