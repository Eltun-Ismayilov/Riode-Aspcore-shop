using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.QuestionModelu
{
    public class QuestionsEditCommand : QuestionsViewModel, IRequest<int>
    {

        public class QuestionsEditCommandHandler : IRequestHandler<QuestionsEditCommand, int>
        {
            readonly RiodeDbContext db;
            readonly IActionContextAccessor ctx;

            public QuestionsEditCommandHandler(RiodeDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<int> Handle(QuestionsEditCommand model, CancellationToken cancellationToken)
            {
                if (model.Id == null || model.Id < 0)

                    return 0;


                var entity = await db.Questions.FirstOrDefaultAsync(b => b.Id == model.Id && b.DeleteByUserId == null);

                if (ctx.ModelStateValid())
                {
                    entity.Question = model.Questions;
                    entity.Answer = model.Answer;
                    await db.SaveChangesAsync(cancellationToken);
                    return entity.Id;
                }


                return 0;
            }
        }
    }
}



