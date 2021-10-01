﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Appcode.Application.Infrastructure;
using Riode.WebUI.Model.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.BrandsModelu
{
    public class BrandRemoveCommand : IRequest<CommandJsonRespons>
    {

        public int? Id { get; set; }


        public class BrandRemoveCommandHandler : IRequestHandler<BrandRemoveCommand, CommandJsonRespons>
        {
            readonly RiodeDbContext db;
            public BrandRemoveCommandHandler(RiodeDbContext db)
            {
                this.db = db;
            }
            public async Task<CommandJsonRespons> Handle(BrandRemoveCommand request, CancellationToken cancellationToken)
            {

                var response = new CommandJsonRespons();


                if (request.Id == null || request.Id < 1)
                {
                    response.Error = true;
                    response.Message = "Mellumat tamligi qorunmayib";
                    goto end;
                }

                var brand = await db.Brands.FirstOrDefaultAsync(m => m.Id == request.Id && m.DeleteByUserId == null);

                if (brand == null)
                {
                    response.Error = true;
                    response.Message = "Melumat movcud deyildir.";
                    goto end;
                }

                brand.DeleteByUserId = 1;
                brand.DeleteData = DateTime.Now;
                await db.SaveChangesAsync(cancellationToken);

                response.Error = false;
                response.Message="ugurlu elemlyat";
                end:
                return response;
            }

        }
    }
}
