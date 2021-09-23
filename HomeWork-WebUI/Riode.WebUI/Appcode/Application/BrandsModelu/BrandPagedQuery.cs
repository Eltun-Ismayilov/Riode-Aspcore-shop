using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using Riode.WebUI.Model.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.BrandsModelu
{
    public class BrandPagedQuery:IRequest<PagedViewModel<Brands>>
    {
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 3;

        public class BrandPagedQueryHandler : IRequestHandler<BrandPagedQuery, PagedViewModel<Brands>>
        {
            readonly RiodeDbContext db;
            public BrandPagedQueryHandler(RiodeDbContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<Brands>> Handle(BrandPagedQuery model, CancellationToken cancellationToken)
            {
                var query = db.Brands.Where(b => b.CreateByUserId == null).AsQueryable(); // silinmemisleri getirir

                //int queryCount = await query.CountAsync(cancellationToken); // silinmemislerin sayni takir

                //var pagedData = await query.Skip((model.Pageindex - 1) * model.PageCount) // skip necensi seyfede,
                //    .Take(model.PageCount) // nece denesini gosdersin.
                //    .ToListAsync(cancellationToken);

                return new PagedViewModel<Brands>(query,model.pageIndex,model.pageSize);
            }
        }
    }
}
