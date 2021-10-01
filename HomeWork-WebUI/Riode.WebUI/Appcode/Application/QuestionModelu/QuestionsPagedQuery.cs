using MediatR;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using Riode.WebUI.Model.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.QuestionModelu
{
    public class QuestionsPagedQuery : IRequest<PagedViewModel<Questions>>
    {
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 2;
        public class QuestionsPagedQueryHandler : IRequestHandler<QuestionsPagedQuery, PagedViewModel<Questions>>
        {
            readonly RiodeDbContext db;
            public QuestionsPagedQueryHandler(RiodeDbContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<Questions>> Handle(QuestionsPagedQuery model, CancellationToken cancellationToken)
            {
                var query = db.Questions.Where(b => b.CreateByUserId == null && b.DeleteByUserId == null).AsQueryable(); // silinmemisleri getirir


                return new PagedViewModel<Questions>(query, model.pageIndex, model.pageSize);

            }
        }
    }
}
    
    

