using MediatR;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.BrandsModelu
{
    public class BrandCreateCommand : IRequest<Brands>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public class BrandCreateCommandHandler : IRequestHandler<BrandCreateCommand, Brands>
        {
            readonly RiodeDbContext db;
            public BrandCreateCommandHandler(RiodeDbContext db)
            {
                this.db = db;
            }
            public async Task<Brands> Handle(BrandCreateCommand model, CancellationToken cancellationToken)
            {
                Brands brands = new Brands();
                brands.Name = model.Name;
                brands.description = model.Description;
                db.Brands.Add(brands);
                await db.SaveChangesAsync(cancellationToken);

                return brands;

            }
        }
    }
}
