using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Riode.WebUI.Appcode.Application.SpecificationModelu;
using Riode.WebUI.Model.DataContexts;
using System.Threading.Tasks;

namespace Riode.WebUI.Areas.Admin.Controllers
{

    [Area("Admin")]

    public class SpecificationController : Controller
    {
        private readonly RiodeDbContext db;
        //arxektura ucun yazilir;
        private readonly IMediator mediator;

        public SpecificationController(RiodeDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator; 
        }

        [Authorize(Policy = "admin.brands.index")]
       
        public async Task<IActionResult> Index(SpecificationPagedQuery request)
        {

            //  ViewBag.Count = db.Specifications.Count();

            var response = await mediator.Send(request);

            return View(response);
        }
        [Authorize(Policy = "admin.brands.Details")]

        //+
        public async Task<IActionResult> Details(SpecificationSingleQuery query)
        {

            //mediatur ucun yazilib;

            //var query = new SpecificationSingleQuery
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
        [Authorize(Policy = "admin.brands.Create")]

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecificationCreateCommand command)
        {

            var model = await mediator.Send(command);

            if (model != null)

                return RedirectToAction(nameof(Index));




            return View(command);
        }

        [Authorize(Policy = "admin.brands.Edit")]

        //+
        public async Task<IActionResult> Edit(SpecificationSingleQuery query)
        {


            var respons = await mediator.Send(query);


            if (respons == null)
            {
                return NotFound();
            }

            SpecificationViewModel vm = new SpecificationViewModel();
            vm.Id = respons.Id;
            vm.Name = respons.Name;
            return View(vm);

        }

        [Authorize(Policy = "admin.brands.Edit")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SpecificationEditCommand command)
        {

            var id = await mediator.Send(command);

            if (id > 0)

                return RedirectToAction(nameof(Index));

            return View(command);
        }



        [Authorize(Policy = "admin.brands.Delete")]

        [HttpPost]
        public async Task<IActionResult> Delete(SpecificationRemoveCommand requst)
        {

            var respons = await mediator.Send(requst);

            return Json(respons);
        }

    }
}
