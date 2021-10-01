using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.OneCategoryModelu
{
    public class CategorySingleQuery : IRequest<OneCategory>
    {
        // bu hisse query model adlanir;(axtaris zamani bura lazim olur)
        public int Id { get; set; }


        // nested class clasin icinde class
        public class CategorySingleQueryHandler : IRequestHandler<CategorySingleQuery, OneCategory>
        {
            readonly RiodeDbContext db;
            public CategorySingleQueryHandler(RiodeDbContext db)
            {
                this.db = db; //Model
            }
            // qeury servic adlanir;    
            public async Task<OneCategory> Handle(CategorySingleQuery model, CancellationToken cancellationToken)
            {

                if (model.Id <= 0)
                {
                    return null;
                }
                var brands = await db.OneCategories
                   .FirstOrDefaultAsync(m => m.Id == model.Id, cancellationToken);

                return brands;
            }
        }

    }
}
