using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.ProductSizeModelu
{
    public class SizeSingleQuery: IRequest<ProductSize>
    {
        public int Id { get; set; }

        public class SizeSingleQueryHandler : IRequestHandler<SizeSingleQuery, ProductSize>

        {
            readonly RiodeDbContext db;
            public SizeSingleQueryHandler(RiodeDbContext db)
            {
                this.db = db; //Model
            }
            public async Task<ProductSize> Handle(SizeSingleQuery model, CancellationToken cancellationToken)
            {
                if (model.Id <= 0)
                {
                    return null;
                }
                var sizes = await db.ProductSizes
                   .FirstOrDefaultAsync(m => m.Id == model.Id, cancellationToken);

                return sizes;
            }
        }
    }
}
