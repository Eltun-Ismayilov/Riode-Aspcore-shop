using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Appcode.Application.BlogsModelu;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [AllowAnonymous]

    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly RiodeDbContext db;
        private readonly IWebHostEnvironment env;
        private readonly IMediator mediator;

        public BlogsController(RiodeDbContext db, IWebHostEnvironment env, IMediator mediator)
        {
            this.db = db;
            this.env = env;
            this.mediator = mediator;
        }

        // GET: Admin/Blogs
        [Authorize(Policy = "admin.Blog.Index")]

        public async Task<IActionResult> Index(BlogsPagedQuery request)
        {

            var response = await mediator.Send(request);

            return View(response);
        }

        [Authorize(Policy = "admin.Blog.Details")]
        public async Task<IActionResult> Details(BlogsSingleQuery query)
        {

            var respons = await mediator.Send(query);

            if (respons == null)
            {
                return NotFound();
            }

            return View(respons);
        }

        [Authorize(Policy = "admin.Blog.Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "admin.Blog.Create")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogsCreateComman command)
        {

            Blog model = await mediator.Send(command);

            if (model != null)

                return RedirectToAction(nameof(Index));

            return View(command);
        }

        [Authorize(Policy = "admin.Blog.Edit")]
        public async Task<IActionResult> Edit(BlogsSingleQuery query)
        {
            var respons = await mediator.Send(query);

            if (respons == null)
            {
                return NotFound();
            }
            BlogsViewModel vm = new BlogsViewModel();
            vm.Id = respons.Id;
            vm.Title = respons.Title;
            vm.Body = respons.Body;
            vm.imagepati = respons.ImagePati;
            return View(vm);


        }

        [Authorize(Policy = "admin.Blog.Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BlogsEditCommand  command)
        {

            var id = await mediator.Send(command);

            if (id > 0)

                return RedirectToAction(nameof(Index));

            return View(command);



        
           
        }

    [Authorize(Policy = "admin.Blog.Delete")]
    [HttpPost]
    public async Task<IActionResult> Delete(BlogsRemoveCommand requst)
    {
        var respons = await mediator.Send(requst);

        return Json(respons);
    }


}
}
