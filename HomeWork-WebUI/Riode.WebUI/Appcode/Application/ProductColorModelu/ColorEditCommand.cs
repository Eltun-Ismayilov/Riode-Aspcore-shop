using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.ProductColorModelu
{
    public class ColorEditCommand : ColorViewModel, IRequest<int>
    {
        //public int? Id { get; set; }
        //[Required]
        //public string Name { get; set; }
        //public string Description { get; set; }

        public class ColorEditCommandHandler : IRequestHandler<ColorEditCommand, int>
        {
            readonly RiodeDbContext db;
            readonly IActionContextAccessor ctx;

            public ColorEditCommandHandler(RiodeDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<int> Handle(ColorEditCommand model, CancellationToken cancellationToken)
            {

                if (model.Id == null || model.Id < 0)

                    return 0;


                var entity = await db.ProductColors.FirstOrDefaultAsync(b => b.Id == model.Id && b.DeleteByUserId == null);

                if (ctx.ModelStateValid())
                {
                    entity.Name = model.Name;
                    entity.description = model.Description;
                    entity.SkuCode = model.SkuCode;
                    await db.SaveChangesAsync(cancellationToken);
                    return entity.Id;
                }


                return 0;
            }
        }
    }
}

