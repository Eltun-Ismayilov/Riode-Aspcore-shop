using MediatR;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using Riode.WebUI.Model.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.OneCategoryModelu
{
    public class CategoryPagedQuery : IRequest<PagedViewModel<OneCategory>>
    {
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 3;

        public class CategoryPagedQueryHandler : IRequestHandler<CategoryPagedQuery, PagedViewModel<OneCategory>>
        {
            readonly RiodeDbContext db;
            public CategoryPagedQueryHandler(RiodeDbContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<OneCategory>> Handle(CategoryPagedQuery model, CancellationToken cancellationToken)
            {
                var query = db.OneCategories.Where(b => b.CreateByUserId == null && b.DeleteByUserId == null).AsQueryable(); // silinmemisleri getirir

                //int queryCount = await query.CountAsync(cancellationToken); // silinmemislerin sayni tapir

                //var pagedData = await query.Skip((model.Pageindex - 1) * model.PageCount) // skip necensi seyfede,
                //    .Take(model.PageCount) // nece denesini gosdersin.
                //    .ToListAsync(cancellationToken);

                return new PagedViewModel<OneCategory>(query, model.pageIndex, model.pageSize);
            }

        }
    }

}