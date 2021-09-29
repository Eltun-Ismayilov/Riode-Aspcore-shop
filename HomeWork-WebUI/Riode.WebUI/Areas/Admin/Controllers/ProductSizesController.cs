﻿using System;
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

        // GET: Admin/ProductSizes
        public async Task<IActionResult> Index(SizePagedQuery request)
        {

            var response = await mediator.Send(request);

            return View(response);
        }

        // GET: Admin/ProductSizes/Details/5
        public async Task<IActionResult> Details(SizeSingleQuery query)
        {


            var respons = await mediator.Send(query);
            if (respons == null)
            {
                return NotFound();
            }


            return View(respons);
        }

        // GET: Admin/ProductSizes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ProductSizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SizeCreateCommand command)
        {


            ProductSize model = await mediator.Send(command);

            if (model != null)

                return RedirectToAction(nameof(Index));




            return View(command);

        }

        // GET: Admin/ProductSizes/Edit/5
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

        // POST: Admin/ProductSizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SizeEditCommand command)
        {
            var id = await mediator.Send(command);

            if (id > 0)

                return RedirectToAction(nameof(Index));

            return View(command);
        }

        // GET: Admin/ProductSizes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSize = await db.ProductSizes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSize == null)
            {
                return NotFound();
            }

            return View(productSize);
        }

        // POST: Admin/ProductSizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productSize = await db.ProductSizes.FindAsync(id);
            productSize.DeleteData = DateTime.Now;
            productSize.DeleteByUserId = 1;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductSizeExists(int id)
        {
            return db.ProductSizes.Any(e => e.Id == id);
        }
    }
}
