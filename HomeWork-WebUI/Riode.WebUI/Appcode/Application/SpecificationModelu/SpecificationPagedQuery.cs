using MediatR;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using Riode.WebUI.Model.Entity.ViewModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.SpecificationModelu
{
    public class SpecificationPagedQuery:IRequest<PagedViewModel<Specification>>
    {
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 5;

        public class SpecificationPagedQueryHandler : IRequestHandler<SpecificationPagedQuery, PagedViewModel<Specification>>
        {
            readonly RiodeDbContext db;
            public SpecificationPagedQueryHandler(RiodeDbContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<Specification>> Handle(SpecificationPagedQuery model, CancellationToken cancellationToken)
            {
                var query = db.Specifications.Where(b => b.CreateByUserId == null && b.DeleteByUserId==null).AsQueryable(); // silinmemisleri getirir

                //int queryCount = await query.CountAsync(cancellationToken); // silinmemislerin sayni takir

                //var pagedData = await query.Skip((model.Pageindex - 1) * model.PageCount) // skip necensi seyfede,
                //    .Take(model.PageCount) // nece denesini gosdersin.
                //    .ToListAsync(cancellationToken);

                return new PagedViewModel<Specification>(query,model.pageIndex,model.pageSize);
            }
        }
    }
}
