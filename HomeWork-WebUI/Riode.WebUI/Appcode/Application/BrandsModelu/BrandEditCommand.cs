using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Riode.WebUI.Model.Entity;
using System.Threading;
using Riode.WebUI.Model.DataContexts;

namespace Riode.WebUI.Appcode.Application.BrandsModelu
{
    public class BrandEditCommand:IRequest<Brands>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }

        public class BrandEditCommandHandler : IRequestHandler<BrandEditCommand, Brands>
        {
            readonly RiodeDbContext db;
            public BrandEditCommandHandler(RiodeDbContext db)
            {
                this.db = db;
            }
            public async Task<Brands> Handle(BrandEditCommand model, CancellationToken cancellationToken)
            {


                Brands brands = new Brands();
                brands.Name = model.Name;
                brands.description = model.Description;
                brands.Id = model.Id;
                db.Brands.Add(brands);
                await db.SaveChangesAsync(cancellationToken);

                return brands;
            }
        }
    }
}
