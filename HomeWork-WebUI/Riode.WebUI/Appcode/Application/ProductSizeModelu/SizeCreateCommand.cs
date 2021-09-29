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

namespace Riode.WebUI.Appcode.Application.ProductSizeModelu
{
    public class SizeCreateCommand : IRequest<ProductSize>
    {
        [Required]
        public string Name { get; set; }
        public string Abbr { get; set; }
        public string Description { get; set; }

        public class SizeCreateCommandHandler : IRequestHandler<SizeCreateCommand, ProductSize>
        {
            readonly RiodeDbContext db;
            readonly IActionContextAccessor ctx;
            public SizeCreateCommandHandler(RiodeDbContext db, IActionContextAccessor ctx) //Model.State burda yoxlamaq ucun yazilib.
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<ProductSize> Handle(SizeCreateCommand model, CancellationToken cancellationToken)
            {
                if (ctx.ModelStateValid())
                {
                    ProductSize size = new ProductSize();
                    size.Name = model.Name;
                    size.description = model.Description;
                    size.Abbr = model.Abbr;
                    db.ProductSizes.Add(size);
                    await db.SaveChangesAsync(cancellationToken);

                    return size;
                }

                return null;
            }
        }
    }
}
