using MediatR;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using Riode.WebUI.Model.Entity.ViewModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.ProductSizeModelu
{
    public class SizePagedQuery : IRequest<PagedViewModel<ProductSize>>
    {
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 2;
        public class SizePagedQueryHandler : IRequestHandler<SizePagedQuery, PagedViewModel<ProductSize>>
        {
            readonly RiodeDbContext db;
            public SizePagedQueryHandler(RiodeDbContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<ProductSize>> Handle(SizePagedQuery model, CancellationToken cancellationToken)
            {
                var query = db.ProductSizes.Where(b => b.CreateByUserId == null && b.DeleteByUserId == null).AsQueryable(); // silinmemisleri getirir


                return new PagedViewModel<ProductSize>(query, model.pageIndex, model.pageSize);

            }
        }
    }
}
