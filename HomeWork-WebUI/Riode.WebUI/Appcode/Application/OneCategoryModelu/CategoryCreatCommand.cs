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
    public class CategoryCreatCommand: IRequest<int>
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParendId { get; set; }

        public class CategoryCreatCommandHandler : IRequestHandler<CategoryCreatCommand, int>
        {
            readonly RiodeDbContext db;
            readonly IActionContextAccessor ctx;
            public CategoryCreatCommandHandler(RiodeDbContext db, IActionContextAccessor ctx) //Model.State burda yoxlamaq ucun yazilib.
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<int> Handle(CategoryCreatCommand model, CancellationToken cancellationToken)
            {

                if (ctx.ModelStateValid())
                {
                    OneCategory category = new OneCategory();
                    category.Name = model.Name;
                    category.Description = model.Description;
                    category.ParentId = model.ParendId;
                    db.OneCategories.Add(category);
                    await db.SaveChangesAsync(cancellationToken);

                    return category.Id;
                }

                return 0;

                //ctx.ActionContext.ModelState.IsValid if icinde bu cur yoxlamamaq ucun extension yaziiriq.
            }
        }
    
    }
}
