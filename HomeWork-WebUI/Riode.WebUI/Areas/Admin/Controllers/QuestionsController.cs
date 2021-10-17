using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Appcode.Application.QuestionModelu;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuestionsController : Controller
    {
        private readonly RiodeDbContext db;
        private readonly IMediator mediator;

        public QuestionsController(RiodeDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

      //  [Authorize(Policy = "admin.Question.Index")]
        public async Task<IActionResult> Index(QuestionsPagedQuery request)
        {
            ViewBag.Count = db.Questions.Count();


            var response = await mediator.Send(request);

            return View(response);
        }

      //  [Authorize(Policy = "admin.Question.Details")]
        public async Task<IActionResult> Details(QuestionsSingleQuery query)
        {
            var respons = await mediator.Send(query);

            if (respons == null)
            {
                return NotFound();
            }

            return View(respons);
        }

      //  [Authorize(Policy = "admin.Question.Create")]
        public IActionResult Create()
        {
            return View();
        }

      //  [Authorize(Policy = "admin.Question.Create")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionCreateCommand command)
        {
            Questions model = await mediator.Send(command);

            if (model != null)

                return RedirectToAction(nameof(Index));




            return View(command);
        }

     //   [Authorize(Policy = "admin.Question.Edit")]
        public async Task<IActionResult> Edit(QuestionsSingleQuery query)
        {
            var respons = await mediator.Send(query);

            if (respons == null)
            {
                return NotFound();
            }

            QuestionsViewModel vm = new QuestionsViewModel();
            vm.Id = respons.Id;
            vm.Questions = respons.Question;
            vm.Answer = respons.Answer;
            return View(vm);
        }

      //  [Authorize(Policy = "admin.Question.Edit")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(QuestionsEditCommand command)
        {
        

                var id = await mediator.Send(command);

                if (id > 0)

                    return RedirectToAction(nameof(Index));

                return View(command);
            }

      //  [Authorize(Policy = "admin.Question.Delete")]
        public async Task<IActionResult> Delete(QuestionsRemoveCommand requst)
        {


            var respons = await mediator.Send(requst);

            return Json(respons);
        }

     
    }
}
