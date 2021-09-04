using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Riode.WebUI.Model.DataContexts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();


            // Patilarin Standart balaca herifnen yazilisi;
            services.AddRouting(cfg => cfg.LowercaseUrls = true);


            // Dependency Injection Isdifade edilmesi ucun yazilmisdir;
            services.AddDbContext<RiodeDbContext>(cfg=>{ });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // devoloper ucun Error cixarilmasi;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();


            app.UseStaticFiles();
            app.UseEndpoints(cfg =>
            {
                // static fayilarin oxunmasi ucun yazilmis kod;

                cfg.MapGet("/coming-soon.html", async (context) =>
                {
                    using (var sr = new StreamReader("views/Static/coming-soon.html"))
                    {
                        context.Response.ContentType = "text/html";
                        await context.Response.WriteAsync(sr.ReadToEnd());
                    }

                });
                cfg.MapControllerRoute("default", "{controller=shop}/{action=index}/{id?}");
            });
        }
    }
}

// Dependency Injection bize komek eliyirki her defe DbContext inistansini(nusxesini) yaratmayaq ozu bize injent elesin;
// her bir contrelleri ozu bizim ucun gondersin,MVC elave edilib kim isdiyir elesin kim isdiyir elemesi,
// 1-ci olaraq ctor yaradiriq(Controller icinde);
// MES

//public HomeController()
//{

//}
// 2-CI Olaraq ctor DataContexts elave edirik;

//public HomeController(RiodeDbContext db)
//{

//}

// 3-cu olaraq hemin DataContexts qlobal vezyete cixariqki basqa action taninsin;

// redonly RiodeDbContext db;

//public HomeController(RiodeDbContext db)
//{
// this.db=db;
//}

// 4-cu ise startupda ConfigureServices method biz  Dependency Injection registasyadan cekirmeliyik;

// yeni  services.AddDbContext<Data Model adi yazmaliyiq>(cfg=<{}, ServicesLifetime.Scoped);

///////////////////////////////////////////////////////
// Sorgu boyunca 1 defe yarat  ServicesLifetime.Scoped yeni her defe sorugu gonderedikde yaranir;
// Proqram(application) boyunca 1 defe yarat; Services.addsingletion yeni proqram yaranaandi ancaq 1 defe instans verir;
// Her muraciet boyunca 1 defe yarat;  services.addTransient yeni seyfede 3 dene eyni email varsa hemise reflesh olunanda hamsinada yari db context verecek;

