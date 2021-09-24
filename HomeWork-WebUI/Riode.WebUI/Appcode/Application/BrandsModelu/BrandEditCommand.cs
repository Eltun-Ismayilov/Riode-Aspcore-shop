﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Riode.WebUI.Model.Entity;
using System.Threading;
using Riode.WebUI.Model.DataContexts;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Riode.WebUI.Appcode.Application.BrandsModelu
{
    public class BrandEditCommand : BrandViewModel, IRequest<int>
    {
    //public int? Id { get; set; }
    //[Required]
    //public string Name { get; set; }
    //public string Description { get; set; }

    public class BrandEditCommandHandler : IRequestHandler<BrandEditCommand, int>
    {
        readonly RiodeDbContext db;
        readonly IActionContextAccessor ctx;

        public BrandEditCommandHandler(RiodeDbContext db, IActionContextAccessor ctx)
        {
            this.db = db;
            this.ctx = ctx;
        }
        public async Task<int> Handle(BrandEditCommand model, CancellationToken cancellationToken)
        {

            if (model.Id == null || model.Id < 0)

                return 0;


            var entity = await db.Brands.FirstOrDefaultAsync(b => b.Id == model.Id && b.DeleteByUserId == null);

                if (ctx.ModelStateValid())
                {
                    entity.Name = model.Name;
                    entity.description = model.Description;
                    await db.SaveChangesAsync(cancellationToken);
                    return entity.Id;
                }


            return 0;
        }
    }
}
}
