using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Appcode;
using Riode.WebUI.Appcode.Application.OneCategoryModelu;
using Riode.WebUI.Model.DataContexts;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.CategoryModule
{
    public class CategoryEditCommand : CategoryViewModel, IRequest<int>
    {
        public class CategoryEditCommandHandler : IRequestHandler<CategoryEditCommand, int>
        {
            readonly RiodeDbContext db;
            readonly IActionContextAccessor ctx;
            public CategoryEditCommandHandler(Model.DataContexts.RiodeDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }

            public async Task<int> Handle(CategoryEditCommand request, CancellationToken cancellationToken)
            {
                if (request.Id == null || request.Id < 0)
                    return 0;

                var entity = await db.OneCategories.FirstOrDefaultAsync(b => b.Id == request.Id && b.DeleteByUserId == null);

                if (entity == null)
                    return 0;

                if (ctx.ModelStateValid())
                {
                    entity.ParentId = request.ParentId;
                    entity.Name = request.Name;
                    entity.Description = request.Description;
                    await db.SaveChangesAsync(cancellationToken);
                    return entity.Id;
                }

                return 0;
            }
        }
    }
}