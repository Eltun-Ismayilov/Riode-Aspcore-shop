using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Appcode.Application.BrandsModelu;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]

    public class BrandsController : Controller
    {
        private readonly RiodeDbContext db;
        //arxektura ucun yazilir;
        private readonly IMediator mediator;

        public BrandsController(RiodeDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }


        public async Task<IActionResult> Index(BrandPagedQuery request)
        {

            //  ViewBag.Count = db.Brands.Count();

            var response = await mediator.Send(request);

            return View(response);
        }

        //+

        public async Task<IActionResult> Details(BrandSingleQuery query)
        {

            //mediatur ucun yazilib;

            //var query = new BrandSingleQuery
            //{
            //    Id = id  belede yaza bilerik 
            //}; 

            var respons = await mediator.Send(query);

            if (respons == null)
            {
                return NotFound();
            }

            return View(respons);
        }

        //+k
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandCreateCommand command)
        {

            Brands model = await mediator.Send(command);

            if (model != null)

                return RedirectToAction(nameof(Index));




            return View(command);
        }


        //+
        public async Task<IActionResult> Edit(BrandSingleQuery query)
        {


            var respons = await mediator.Send(query);


            if (respons == null)
            {
                return NotFound();
            }

            BrandViewModel vm = new BrandViewModel();
            vm.Id = respons.Id;
            vm.Name = respons.Name;
            vm.Description = respons.description;
            return View(vm);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandEditCommand command)
        {

            var id = await mediator.Send(command);

            if (id > 0)

                return RedirectToAction(nameof(Index));

            return View(command);
        }





        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brands = await db.Brands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brands == null)
            {
                return NotFound();
            }

            return View(brands);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brands = await db.Brands.FindAsync(id);

            brands.DeleteData = DateTime.Now;
            brands.DeleteByUserId = 1;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool BrandsExists(int id)
        {
            return db.Brands.Any(e => e.Id == id);
        }
    }
}
