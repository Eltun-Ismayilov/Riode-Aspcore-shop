using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.BlogsModelu
{
    public class BlogsSingleQuery: IRequest<Blog>
    {
        // bu hisse query model adlanir;(axtaris zamani bura lazim olur)
        public int Id { get; set; }


        // nested class clasin icinde class
        public class BrandSingleQueryHandler : IRequestHandler<BlogsSingleQuery, Blog>
        {
            readonly RiodeDbContext db;
            public BrandSingleQueryHandler(RiodeDbContext db)
            {
                this.db = db; //Model
            }
            // qeury servic adlanir;    
            public async Task<Blog> Handle(BlogsSingleQuery model, CancellationToken cancellationToken)
            {

                if (model.Id <= 0)
                {
                    return null;
                }
                var Blogs = await db.Blogs
                   .FirstOrDefaultAsync(m => m.Id == model.Id, cancellationToken);

                return Blogs;
            }
        }
    }

}


