using MediatR;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using Riode.WebUI.Model.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.ProductColorModelu
{
    public class ColorPagedQuery: IRequest<PagedViewModel<ProductColor>>
    {
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 2;

        public class ColorPagedQueryHandler : IRequestHandler<ColorPagedQuery, PagedViewModel<ProductColor>>
        {
            readonly RiodeDbContext db;
            public ColorPagedQueryHandler(RiodeDbContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<ProductColor>> Handle(ColorPagedQuery model, CancellationToken cancellationToken)
            {
                var query = db.ProductColors.Where(b => b.CreateByUserId == null && b.DeleteByUserId == null).AsQueryable(); // silinmemisleri getirir

                //int queryCount = await query.CountAsync(cancellationToken); // silinmemislerin sayni takir

                //var pagedData = await query.Skip((model.Pageindex - 1) * model.PageCount) // skip necensi seyfede,
                //    .Take(model.PageCount) // nece denesini gosdersin.
                //    .ToListAsync(cancellationToken);

                return new PagedViewModel<ProductColor>(query, model.pageIndex, model.pageSize);
            }
        }
    }
}
