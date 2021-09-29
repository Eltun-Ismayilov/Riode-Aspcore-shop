using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.ProductSizeModelu
{
    public class SizeEditCommand: SizeViewModel, IRequest<int>
    {

        public class SizeEditCommandHandler : IRequestHandler<SizeEditCommand, int>
        {
            readonly RiodeDbContext db;
            readonly IActionContextAccessor ctx;

            public SizeEditCommandHandler(RiodeDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<int> Handle(SizeEditCommand model, CancellationToken cancellationToken)
            {
                if (model.Id == null || model.Id < 0)

                    return 0;


                var entity = await db.ProductSizes.FirstOrDefaultAsync(b => b.Id == model.Id && b.DeleteByUserId == null);

                if (ctx.ModelStateValid())
                {
                    entity.Name = model.Name;
                    entity.description = model.Description;
                    entity.Abbr = model.Abbr;
                    await db.SaveChangesAsync(cancellationToken);
                    return entity.Id;
                }


                return 0;
            }
        }
    }
}
