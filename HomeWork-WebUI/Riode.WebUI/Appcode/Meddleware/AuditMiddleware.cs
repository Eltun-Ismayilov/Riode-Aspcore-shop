using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using System;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Meddleware
{
    public class AuditMiddleware
    {
        private readonly RequestDelegate rd;

        public AuditMiddleware(RequestDelegate rd)
        {
            this.rd = rd;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using (var scope =httpContext.RequestServices.CreateScope())
            {
                RiodeDbContext db = scope.ServiceProvider.GetRequiredService<RiodeDbContext>();


                var ruoteData = httpContext.GetRouteData();

                AuditLog log = new AuditLog();

                log.CreateData = DateTime.Now; // Yaranma tarixi indiki zaman gotursun.

                log.RequestTime = DateTime.Now;  // Sorgunun tarixi

                log.IsHttps = httpContext.Request.IsHttps;  //sorgunun https olub olmadigni yoxluyuruq

                log.Method = httpContext.Request.Method;   // Sorgunun GET veya POST oldugnu yoxluyaq

                log.Pati = httpContext.Request.Path;    // unvan Tamadi gosderir.

            

                if (ruoteData.Values.TryGetValue("area", out object area))   //Area nedi.
                {
                    log.Area = area.ToString();
                }



                if (ruoteData.Values.TryGetValue("controller", out object controller))   //controller nedi.
                {
                    log.Controller = controller.ToString();
                }


                if (ruoteData.Values.TryGetValue("action", out object action))   //action nedi.
                {
                    log.Action = action.ToString();
                }


                if (!string.IsNullOrWhiteSpace(httpContext.Request.QueryString.Value)) // Pati beraber hansi queryString gelib bize onu goturmeliyik
                {
                    log.QueryString = httpContext.Request.QueryString.Value;
                }

                await rd(httpContext);

                log.StatusCode = httpContext.Response.StatusCode; // Status Codun ne oldugnu gotureciyik

                log.RequestTime = DateTime.Now; // Sorgunun cvb tarixi.


                db.AuditLogs.Add(log);
                db.SaveChanges();
            }




        }
    }

    public static class AuditMiddlewareExtensions
    {
        public static IApplicationBuilder UseAudit(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuditMiddleware>();
        }
    }
}

// bunu biz seyfede kimin hara girmesini nezaret etmek ucun isdifade edirik.