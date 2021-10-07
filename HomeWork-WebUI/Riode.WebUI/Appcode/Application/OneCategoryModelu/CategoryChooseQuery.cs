using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.CategoryModule
{
    public class CategoryChooseQuery : IRequest<List<OneCategory>>
    {
        public class CategoryChooseQueryHandler : IRequestHandler<CategoryChooseQuery, List<OneCategory>>
        {
            readonly RiodeDbContext db;
            public CategoryChooseQueryHandler(RiodeDbContext db)
            {
                this.db = db;
            }

            public async Task<List<OneCategory>> Handle(CategoryChooseQuery request, CancellationToken cancellationToken)
            {
                var categories = await db.OneCategories.ToListAsync();

                return categories;
            }
        }
    }

}