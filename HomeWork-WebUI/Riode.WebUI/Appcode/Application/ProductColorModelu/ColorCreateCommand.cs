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

namespace Riode.WebUI.Appcode.Application.ProductColorModelu
{
    public class ColorCreateCommand : IRequest<ProductColor>
    {
        [Required]
        public string Name { get; set; }
        public string SkuCode { get; set; }
        public string Description { get; set; }

        public class ColorCreateCommandHandler : IRequestHandler<ColorCreateCommand, ProductColor>
        {
            readonly RiodeDbContext db;
            readonly IActionContextAccessor ctx;
            public ColorCreateCommandHandler(RiodeDbContext db, IActionContextAccessor ctx) //Model.State burda yoxlamaq ucun yazilib.
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<ProductColor> Handle(ColorCreateCommand model, CancellationToken cancellationToken)
            {


                if (ctx.ModelStateValid())
                {
                    ProductColor colors = new ProductColor();
                    colors.Name = model.Name;
                    colors.SkuCode = model.SkuCode;
                    colors.description = model.Description;
                    db.ProductColors.Add(colors);
                    await db.SaveChangesAsync(cancellationToken);

                    return colors;
                }

                return null;

                //ctx.ActionContext.ModelState.IsValid if icinde bu cur yoxlamamaq ucun extension yaziiriq.
            }

        }
    }
}