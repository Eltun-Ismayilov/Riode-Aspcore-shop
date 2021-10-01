using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.QuestionModelu
{
    public class QuestionsSingleQuery : IRequest<Questions>
    {
        public int Id { get; set; }

        public class QuestionsSingleQueryHandler : IRequestHandler<QuestionsSingleQuery, Questions>

        {
            readonly RiodeDbContext db;
            public QuestionsSingleQueryHandler(RiodeDbContext db)
            {
                this.db = db; //Model
            }
            public async Task<Questions> Handle(QuestionsSingleQuery model, CancellationToken cancellationToken)
            {
                if (model.Id <= 0)
                {
                    return null;
                }
                var sizes = await db.Questions
                   .FirstOrDefaultAsync(m => m.Id == model.Id, cancellationToken);

                return sizes;
            }
        }
    }

}

