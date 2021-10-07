//using MediatR;
//using Riode.WebUI.Model.DataContexts;
//using Riode.WebUI.Model.Entity;
//using Riode.WebUI.Model.Entity.ViewModels;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Riode.WebUI.Appcode.Application.OneCategoryModelu
//{
//    public class CategoryPagedQuery : IRequest<PagedViewModel<List<OneCategory>>>
//    {
//        public int pageIndex { get; set; } = 1;
//        public int pageSize { get; set; } = 2;

//        public class CategoryPagedQueryHandler : IRequestHandler<CategoryPagedQuery, PagedViewModel<List<OneCategory>>>
//        {
//            readonly RiodeDbContext db;
//            public CategoryPagedQueryHandler(RiodeDbContext db)
//            {
//                this.db = db;
//            }
//            public async Task<PagedViewModel<List<OneCategory>>> Handle(CategoryPagedQuery model, CancellationToken cancellationToken)
//            {
//                var query = db.OneCategories.Where(b => b.CreateByUserId == null && b.DeleteByUserId == null).AsQueryable(); // silinmemisleri getirir

//                //int queryCount = await query.CountAsync(cancellationToken); // silinmemislerin sayni tapir

//                //var pagedData = await query.Skip((model.Pageindex - 1) * model.PageCount) // skip necensi seyfede,
//                //    .Take(model.PageCount) // nece denesini gosdersin.
//                //    .ToListAsync(cancellationToken);

//                return new PagedViewModel<List<OneCategory>>(query, model.pageIndex, model.pageSize);
//            }

//        }
//    }

//}



using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.CategoryModule
{
    public class CategoryPagedQuery : IRequest<List<OneCategory>>
    {
        public class CategoryPagedQueryHandler : IRequestHandler<CategoryPagedQuery, List<OneCategory>>
        {
            readonly RiodeDbContext db;
            public CategoryPagedQueryHandler(RiodeDbContext db)
            {
                this.db = db;
            }

            public async Task<List<OneCategory>> Handle(CategoryPagedQuery request, CancellationToken cancellationToken)
            {
                var categories = await db.OneCategories.Where(c => c.DeleteByUserId == null).ToListAsync();

                return categories;
            }
        }
    }

}