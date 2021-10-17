using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Appcode.Application.ProductColorModelu;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductColorsController : Controller
    {
        private readonly RiodeDbContext db;
        private readonly IMediator mediator;


        public ProductColorsController(RiodeDbContext db, IMediator mediator)
        {
           this.db = db;
           this.mediator = mediator;
        }

      //  [Authorize(Policy = "admin.ProductColor.Index")]
        public async Task<IActionResult> Index(ColorPagedQuery request)
        {
            ViewBag.Count = db.ProductColors.Count();

            var response = await mediator.Send(request);

            return View(response);
        }

      //  [Authorize(Policy = "admin.ProductColor.Details")]

        public async Task<IActionResult> Details(ColorSingleQuery query)
        {
            var respons = await mediator.Send(query);

            if (respons == null)
            {
                return NotFound();
            }

            return View(respons);
        }

     //   [Authorize(Policy = "admin.ProductColor.Create")]
        public IActionResult Create()
        {
            return View();
        }

       // [Authorize(Policy = "admin.ProductColor.Create")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ColorCreateCommand command)
        {
            ProductColor model = await mediator.Send(command);

            if (model != null)

                return RedirectToAction(nameof(Index));




            return View(command);
        }

      //  [Authorize(Policy = "admin.ProductColor.Edit")]
        public async Task<IActionResult> Edit(ColorSingleQuery query)
        {

            var respons = await mediator.Send(query);


            if (respons == null)
            {
                return NotFound();
            }

            ColorViewModel vm = new ColorViewModel();
            vm.Id = respons.Id;
            vm.Name = respons.Name;
            vm.SkuCode = respons.SkuCode;
            vm.Description = respons.description;
            return View(vm);
        }


      //  [Authorize(Policy = "admin.ProductColor.Edit")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ColorEditCommand command)
        {
            var id = await mediator.Send(command);

            if (id > 0)

                return RedirectToAction(nameof(Index));

            return View(command);
        }

      //  [Authorize(Policy = "admin.ProductColor.Delete")]
        [HttpPost]

        public async Task<IActionResult> Delete(ColorRemoveCommand requst)
        {
            var respons = await mediator.Send(requst);

            return Json(respons);
        }

    }
}
