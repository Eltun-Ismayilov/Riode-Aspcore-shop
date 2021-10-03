using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.BrandsModelu
{
    public class BrandCreateCommand : IRequest<Brands>
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public class BrandCreateCommandHandler : IRequestHandler<BrandCreateCommand, Brands>
        {
            readonly RiodeDbContext db;
            readonly IActionContextAccessor ctx;
            public BrandCreateCommandHandler(RiodeDbContext db,IActionContextAccessor ctx) //Model.State burda yoxlamaq ucun yazilib.
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<Brands> Handle(BrandCreateCommand model, CancellationToken cancellationToken)
            {


                if (ctx.ModelStateValid()) 
                {
                    Brands brands = new Brands();
                    brands.Name = model.Name;
                    brands.description = model.Description;
                    db.Brands.Add(brands);
                    await db.SaveChangesAsync(cancellationToken);

                    return brands;
                }

                return null;

                //ctx.ActionContext.ModelState.IsValid if icinde bu cur yoxlamamaq ucun extension yaziiriq.
            }
        }
    }
}
