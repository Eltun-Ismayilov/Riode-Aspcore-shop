using MediatR;
using Microsoft.EntityFrameworkCore;
//using Riode.WebUI.Migrations;
using Riode.WebUI.Model.DataContexts;
using System.Threading;
using System.Threading.Tasks;
using Riode.WebUI.Model.Entity;

namespace Riode.WebUI.Appcode.Application.SpecificationModelu
{
    public class SpecificationSingleQuery : IRequest<Specification>
    {
        // bu hisse query model adlanir;(axtaris zamani bura lazim olur)
        public int Id { get; set; }


        // nested class clasin icinde class
        public class SpecificationSingleQueryHandler : IRequestHandler<SpecificationSingleQuery, Specification>
        {
            readonly RiodeDbContext db;
            public SpecificationSingleQueryHandler(RiodeDbContext db)
            {
                this.db = db; //Model
            }
            // qeury servic adlanir;    
            public async Task<Specification> Handle(SpecificationSingleQuery model, CancellationToken cancellationToken)
            {

                if (model.Id <= 0)
                {
                    return null;
                }
                var brands = await db.Specifications
                   .FirstOrDefaultAsync(m => m.Id == model.Id, cancellationToken);

                return brands;
            }
        }
    }

}
//Bu proses ile biz Details ve Edit melumatlari gedire bilirik;
// ve bu prossesler ancaq GET action gedir;