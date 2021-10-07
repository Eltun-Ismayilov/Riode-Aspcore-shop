using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Appcode.Application.OneCategoryModelu;
using Riode.WebUI.AppCode.Application.CategoryModule;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [AllowAnonymous]

    [Area("Admin")]
    public class OneCategoriesController : Controller
    {
        private readonly RiodeDbContext db;
        private readonly IMediator mediator;

        public OneCategoriesController(RiodeDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        [Authorize(Policy = "admin.OneCategory.Index")]
        public async Task<IActionResult> Index(CategoryPagedQuery request)
        {


            ViewBag.Count = db.OneCategories.Count();
            var response = await mediator.Send(request);

            return View(response);
            //   return View(await db.OneCategories.Where(c => c.DeleteByUserId == null).ToListAsync());
        }
        [Authorize(Policy = "admin.OneCategory.Details")]//+
        public async Task<IActionResult> Details(CategorySingleQuery query)
        {
            var respons = await mediator.Send(query);

            if (respons == null)
            {
                return NotFound();
            }

            return View(respons);
        }

        [Authorize(Policy = "admin.OneCategory.Create")]

        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(db.OneCategories, "Id", "Name");
            return View();
        }



        [Authorize(Policy = "admin.OneCategory.Create")]

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreatCommand command)
        {

            if (ModelState.IsValid)
            {
                await mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }

            ViewData["ParentId"] = new SelectList(await mediator.Send(new CategoryChooseQuery()), "Id", "Name", command.ParendId);
            return View(command);
        }

        [Authorize(Policy = "admin.OneCategory.Edit")]
        public async Task<IActionResult> Edit(CategorySingleQuery query)
        {
            if (query?.Id == null)
                return NotFound();

            var response = await mediator.Send(query);

            if (response == null)
                return NotFound();

            ViewData["ParentId"] = new SelectList(await mediator.Send(new CategoryChooseQuery()), "Id", "Name", response.ParentId);

            CategoryViewModel vm = new CategoryViewModel();
            vm.Id = response.Id;
            vm.Name = response.Name;
            vm.Description = response.Description;
            vm.ParentId = response.ParentId;
            return View(vm);

        }

        [Authorize(Policy = "admin.OneCategory.Edit")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CategoryEditCommand command)
        {

            if (id != command.Id)
                return NotFound();


            if (ModelState.IsValid)
            {
                await mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(await mediator.Send(new CategoryChooseQuery()), "Id", "Name", command.ParentId);
            return View(command);
        }

        [Authorize(Policy = "admin.OneCategory.Delete")]//+
        [HttpPost]
        public async Task<IActionResult> Delete(CategoryRemoveCommand requst)
        {
            var respons = await mediator.Send(requst);

            return Json(respons);
        }



    }
}
