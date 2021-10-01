using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Application.BlogsModelu
{
    public class BlogsEditCommand : BlogsViewModel, IRequest<int>
    {


        public class BlogsEditCommandHandler : IRequestHandler<BlogsEditCommand, int>
        {
            readonly RiodeDbContext db;
            readonly IActionContextAccessor ctx;
            readonly IWebHostEnvironment env;


            public BlogsEditCommandHandler(RiodeDbContext db, IActionContextAccessor ctx, IWebHostEnvironment env)
            {
                this.db = db;
                this.ctx = ctx;
                this.env = env;
            }
            public async Task<int> Handle(BlogsEditCommand model, CancellationToken cancellationToken)
            {
                Blog blog = new Blog();

                //if (model.Id != blog.Id)
                //{
                //    return null;
                //}

                //if (string.IsNullOrWhiteSpace(model.fileTemp) && model.file == null)
                //{
                //    ModelState.AddModelError("file", "sekil secilmeyib");
                //}


                if (ctx.ModelStateValid())
                {
                    
                        //db.Update(blog);

                        var entity = await db.Blogs.FirstOrDefaultAsync(b => b.Id == model.Id && b.DeleteByUserId == null);

                        model.Title = blog.Title;
                       model.Body = blog.Body;


                        if (model.file != null)
                        {

                            string extension = Path.GetExtension(model.file.FileName);  //.jpg tapmaq ucundur.

                            blog.ImagePati = $"{Guid.NewGuid()}{extension}";//imagenin name 


                            string phsicalFileName = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", "blog", "mask", blog.ImagePati);

                            using (var stream = new FileStream(phsicalFileName, FileMode.Create, FileAccess.Write))
                            {
                                await model.file.CopyToAsync(stream);
                            }

                            if (!string.IsNullOrWhiteSpace(entity.ImagePati))
                            {
                                System.IO.File.Delete(Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", "blog", "mask", entity.ImagePati));

                            }
                            entity.ImagePati = blog.ImagePati;
                        }

                        await db.SaveChangesAsync();



                }
                return 0;

            }

        }
    }
}

