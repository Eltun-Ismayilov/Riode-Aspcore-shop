using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Appcode.Application.ProductSizeModelu;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [AllowAnonymous]

    [Area("Admin")]
    public class ProductSizesController : Controller
    {
        private readonly RiodeDbContext db;
        private readonly IMediator mediator;

        public ProductSizesController(RiodeDbContext db, IMediator mediator)
        {
           this.db = db;
           this.mediator = mediator;
        }

        [Authorize(Policy = "admin.ProductSize.Index")]
        public async Task<IActionResult> Index(SizePagedQuery request)
        {

            var response = await mediator.Send(request);

            return View(response);
        }

        [Authorize(Policy = "admin.ProductSize.Details")]
        public async Task<IActionResult> Details(SizeSingleQuery query)
        {


            var respons = await mediator.Send(query);
            if (respons == null)
            {
                return NotFound();
            }


            return View(respons);
        }

        [Authorize(Policy = "admin.ProductSize.Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "admin.ProductSize.Create")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SizeCreateCommand command)
        {


            ProductSize model = await mediator.Send(command);

            if (model != null)

                return RedirectToAction(nameof(Index));




            return View(command);

        }

        [Authorize(Policy = "admin.ProductSize.Edit")]

        public async Task<IActionResult> Edit(SizeSingleQuery query)
        {

            var respons = await mediator.Send(query);
            if (respons == null)
            {
                return NotFound();
            }
            SizeViewModel vm = new SizeViewModel();
            vm.Id = respons.Id;
            vm.Name = respons.Name;
            vm.Description = respons.description;
            vm.Abbr = respons.Abbr;
            return View(vm);

        }
        [Authorize(Policy = "admin.ProductSize.Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SizeEditCommand command)
        {
            var id = await mediator.Send(command);

            if (id > 0)

                return RedirectToAction(nameof(Index));

            return View(command);
        }

        [Authorize(Policy = "admin.ProductSize.Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(SizeRemoveCommand requst)
        {

            var respons = await mediator.Send(requst);

            return Json(respons);
        }

       
    }
}
