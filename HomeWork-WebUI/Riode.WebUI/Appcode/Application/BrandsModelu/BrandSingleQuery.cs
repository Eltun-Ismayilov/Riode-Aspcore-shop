using MediatR;
using Microsoft.EntityFrameworkCore;
//using Riode.WebUI.Migrations;
using Riode.WebUI.Model.DataContexts;
using System.Threading;
using System.Threading.Tasks;
using Riode.WebUI.Model.Entity;

namespace Riode.WebUI.Appcode.Application.BrandsModelu
{
    public class BrandSingleQuery : IRequest<Brands>
    {
        // bu hisse query model adlanir;(axtaris zamani bura lazim olur)
        public int Id { get; set; }


        // nested class clasin icinde class
        public class BrandSingleQueryHandler : IRequestHandler<BrandSingleQuery, Brands>
        {
            readonly RiodeDbContext db;
            public BrandSingleQueryHandler(RiodeDbContext db)
            {
                this.db = db; //Model
            }
            // qeury servic adlanir;    
            public async Task<Brands> Handle(BrandSingleQuery model, CancellationToken cancellationToken)
            {

                if (model.Id <= 0)
                {
                    return null;
                }
                var brands = await db.Brands
                   .FirstOrDefaultAsync(m => m.Id == model.Id, cancellationToken);

                return brands;
            }
        }
    }

}
//Bu proses ile biz Details ve Edit melumatlari gedire bilirik;
// ve bu prossesler ancaq GET action gedir;