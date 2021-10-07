using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.SpecificationModelu
{
    public class SpecificationCreateCommand : IRequest<Specification>
    {
        [Required]
        public string Name { get; set; }
        public class SpecificationCreateCommandHandler : IRequestHandler<SpecificationCreateCommand, Specification>
        {
            readonly RiodeDbContext db;
            readonly IActionContextAccessor ctx;
            public SpecificationCreateCommandHandler(RiodeDbContext db,IActionContextAccessor ctx) //Model.State burda yoxlamaq ucun yazilib.
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<Specification> Handle(SpecificationCreateCommand model, CancellationToken cancellationToken)
            {


                if (ctx.ModelStateValid()) 
                {
                    Specification brands = new Specification();
                    brands.Name = model.Name;
                    db.Specifications.Add(brands);
                    await db.SaveChangesAsync(cancellationToken);

                    return brands;
                }

                return null;

                //ctx.ActionContext.ModelState.IsValid if icinde bu cur yoxlamamaq ucun extension yaziiriq.
            }
        }
    }
}
