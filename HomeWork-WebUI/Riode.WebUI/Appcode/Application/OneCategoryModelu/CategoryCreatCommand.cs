using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.OneCategoryModelu
{
    public class CategoryCreatCommand: IRequest<OneCategory>
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Parend { get; set; }

        public class CategoryCreatCommandHandler : IRequestHandler<CategoryCreatCommand, OneCategory>
        {
            readonly RiodeDbContext db;
            readonly IActionContextAccessor ctx;
            public CategoryCreatCommandHandler(RiodeDbContext db, IActionContextAccessor ctx) //Model.State burda yoxlamaq ucun yazilib.
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<OneCategory> Handle(CategoryCreatCommand model, CancellationToken cancellationToken)
            {

                if (ctx.ModelStateValid())
                {
                    OneCategory brands = new OneCategory();
                    brands.Name = model.Name;
                    brands.Description = model.Description;
                    brands.ParentId = model.Parend;
                    db.OneCategories.Add(brands);
                    await db.SaveChangesAsync(cancellationToken);

                    return brands;
                }

                return null;

                //ctx.ActionContext.ModelState.IsValid if icinde bu cur yoxlamamaq ucun extension yaziiriq.
            }
        }
    
    }
}
