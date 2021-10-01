using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.QuestionModelu
{
    public class QuestionCreateCommand : IRequest<Questions>
    {
        [Required]
        public string Questions { get; set; }
        [Required]
        public string Answer { get; set; }

        public class SizeCreateCommandHandler : IRequestHandler<QuestionCreateCommand, Questions>
        {
            readonly RiodeDbContext db;
            readonly IActionContextAccessor ctx;
            public SizeCreateCommandHandler(RiodeDbContext db, IActionContextAccessor ctx) //Model.State burda yoxlamaq ucun yazilib.
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<Questions> Handle(QuestionCreateCommand model, CancellationToken cancellationToken)
            {
                if (ctx.ModelStateValid())
                {
                    Questions qesu = new Questions();
                    qesu.Question = model.Questions;
                    qesu.Answer = model.Answer;
                    db.Questions.Add(qesu);
                    await db.SaveChangesAsync(cancellationToken);

                    return qesu;
                }

                return null;
            }
        }

    }
}
