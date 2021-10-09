using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Riode.WebUI.Appcode.Meddleware;
using Riode.WebUI.Appcode.Provider;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity.Membership;
using System;
using System.IO;

namespace Riode.WebUI
{
    public class Startup
    {

        readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;


            // string mykey = "Riode";

            // string plaintext = "text";

            //  string chiperText = "test".Encrypt(mykey);  // WbQPS69gQXY=

            // string myPlainText = "kFVbSlCtCkE=".Decrypte(mykey);


            //string finiw = plaintext.Tomd5();


        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(cfg =>
            {
                //+//
                //Custom ModelBindg cagrilmasi bu curdur;
                cfg.ModelBinderProviders.Insert(0, new BooleanBinderProvider());


                //+//
                //Membership ucun yazilb seyfeye giris edende login olmuyubsansa get login ol sonra gel +
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                cfg.Filters.Add(new AuthorizeFilter(policy));


            })


                //+//
                // productlari filter etmek ucundur loop olmasin(Microsoft.AspNetCore.Mvc.NewtonsoftJson+)+
                .AddNewtonsoftJson(cfg =>
                {
                    cfg.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });



            //+//
            // Patilarin Standart balaca herifnen yazilisi+
            services.AddRouting(cfg => cfg.LowercaseUrls = true);





            // Dependency Injection Isdifade edilmesi ucun yazilmisdir+
            services.AddDbContext<RiodeDbContext>(cfg =>
            {

                // ve burda cagirib yaziriq appsettings adini 
                cfg.UseSqlServer(configuration.GetConnectionString("cString"));

            },ServiceLifetime.Scoped);



            //+//
            //Mediatr dvij elemek(CQRS)
            services.AddMediatR(this.GetType().Assembly);
            //Mediatr Commanlarda Create olunanda isvalid yazmaq ucun yazilib                
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();


            //+//
            //Membership ucun yazilmis kod(Datazbazaya bax)
            //(Microsoft.AspNetCore.Identity.EntityFrameworkCore+)
            services.AddIdentity<RiodeUser, RiodeRole>()
               .AddEntityFrameworkStores<RiodeDbContext>().AddDefaultTokenProviders(); 

            services.AddScoped<UserManager<RiodeUser>>(); // user idare etmek ucun menecer
            services.AddScoped<SignInManager<RiodeUser>>(); //giriw edende idare etmek ucun menecer;
            services.AddScoped<RoleManager<RiodeRole>>(); //role idare etmek ucun menecer;

            services.Configure<IdentityOptions>(cfg =>
            {

                cfg.Password.RequireDigit = false; //Reqem teleb elesin?
                cfg.Password.RequireUppercase = false; //Boyuk reqem teleb elesin?
                cfg.Password.RequireLowercase = false; //Kick reqem teleb elesin?
                cfg.Password.RequiredUniqueChars = 1; //Tekrarlanmiyan nece  sombol olsun?(11-22-3)
                cfg.Password.RequireNonAlphanumeric = false; // 0-9 a-z A-Z  Olmayanlari teleb elemesin?
                cfg.Password.RequiredLength = 3; //Password nece simboldan ibaret olsun?

                cfg.User.RequireUniqueEmail = true; //Email tekrarlanmasin 1 adam ucun?
                //cfg.User.AllowedUserNameCharacters = ""; //User neleri isdifade eliye biler?

                cfg.Lockout.MaxFailedAccessAttempts = 3;// 3 seferden cox sefh giris etse diyansin?
                cfg.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 5, 0);//Nece deq gozlesin ?
               
             
            });

            services.ConfigureApplicationCookie(cfg =>
            {

                cfg.LoginPath = "/signin.html"; //Eger adam login olunmuyubsa hara gondersin?

                cfg.AccessDeniedPath = "/accessdenied.html";//Senin icazen var bu linke yeni link atanda gire bilmesin diye (yeni fb nese atanda ve ya tiktokda olanda beyenmek olmur zad)

                cfg.ExpireTimeSpan = new TimeSpan(0, 10, 10);//Seni sayitda nece deq saxlasin eger sen hecne elemirsense atacaq yeni login olduqdan sonra diansan ve ya saty girdikden sonra diansan

                cfg.Cookie.Name = "riode"; //Cookie adi ne olsun isdediyin adi yaza bilersen;

            });

            services.AddAuthentication(); //Sayita girmek demekdi(login olmaq)

            services.AddAuthorization(cfg => // Sayitda isdediymizi etmek yeni rola gorede gagas.
            {
                //Action usdunda yazilmis Policy goturmek ucundur.
                foreach (var item in Program.principals)
                {

                    cfg.AddPolicy(item,

                        p =>
                        {
                            p.RequireAssertion(h =>
                            {
                                return h.User.IsInRole("SuperAdmin") || h.User.HasClaim(item,"1");

                            });

                        });
                }
            }); // Sayitda ne ede bilmekdi yeni admin fikirlew;(Salahiyyetli olmaq)(siyahida gorunenleri create,delete ,update,edit elemekdir)
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {



            //+//
            // devoloper ucun Error cixarilmasi
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            //Membership admin yaratmaq ucun yazilibdir...
            // app.SeedMembership();



            app.UseRouting();


            //Membership ucun yazilib admin giris olmadiqda bize user sing atmasin deye
            app.Use(async (context, next) =>
            {
                if (!context.Request.Cookies.ContainsKey("riode")
                && context.Request.RouteValues.TryGetValue("area", out object areaName)
                && areaName.ToString().ToLower().Equals("admin"))
                {
                    var attr = context.GetEndpoint().Metadata.GetMetadata<AllowAnonymousAttribute>();
                    if (attr == null)
                    {

                        context.Response.Redirect("/admin/singin.html");
                        await context.Response.CompleteAsync();

                    }

                }
                await next();
            });



            //+//
            //Membership ucundur ve burada yazilmalidir 'ruotin' asagida 'UseEndpoints' yuxarida;
            app.UseAuthentication();
            app.UseAuthorization();





            //+//
            // static fayilarin oxunmasi ucun yazilmis kod+
            app.UseStaticFiles();


            //+//
            //Meddleware cagrilma mentiqi bu curdur..(Auditlog)('ruotin' asagida 'UseEndpoints' yuxarida)
            app.UseAudit();



            //MultiLang Bizim CulterProvider ucun yazilmis codur.
            app.UseRequestLocalization(cfg =>
            {
                cfg.AddSupportedUICultures("az", "en");
                cfg.AddSupportedCultures("az", "en");


                cfg.RequestCultureProviders.Clear();
                cfg.RequestCultureProviders.Add(new CultureProvider());

            });

            app.UseEndpoints(cfg =>
            {

                //+//
                // static fayilarin oxunmasi ucun yazilmis kod+
                cfg.MapGet("/coming-soon.html", async (context) =>
                {
                    using (var sr = new StreamReader("views/Static/coming-soon.html"))
                    {
                        context.Response.ContentType = "text/html";
                        await context.Response.WriteAsync(sr.ReadToEnd());
                    }

                });


                //+//
                //Membersip ucun yazmisiq routda olanda myaccount/singin yox html kimi singin.html cixsin diye yaziriq;+(admin singin atsin bizi)
                cfg.MapControllerRoute("adminsingin", "admin/singin.html",
                  defaults: new
                  {
                      controller = "Account",
                      action = "singin",
                      area = "Admin"
                  });



                //Membersip ucun yazmisiq routda olanda Account/Logout yox html kimi Logout.html cixsin diye yaziriq;+
                cfg.MapControllerRoute("admin-Logout", "admin/logout.html",
                  defaults: new
                  {
                      controller = "Account",
                      action = "logout",
                      area = "Admin"
                  });




                //+//
                //Membersip ucun yazmisiq routda olanda myaccount/singin yox html kimi singin.html cixsin diye yaziriq;+
                cfg.MapControllerRoute("x", "signin.html",
                  defaults: new
                  {
                      controller = "MyAccount",
                      action = "singIn",
                      area = ""
                  });
                //+//



                //Multilanguc ucun yazilib(User-teref)
                cfg.MapControllerRoute("default-lang-userApplication", "{lang}/{controller=home}/{action=index}/{id?}",
                    constraints: new
                    {
                        lang = "en|az|ru"
                    });


                //MultiLangun ucun yazilib routda  en|ru|az   yazanda islesin diye {lang} yazilir areadan evvel;+
                cfg.MapControllerRoute(
                name: "areas-lang-adminApplication",
                pattern: "{lang}/{area:exists}/{controller=Dashboard}/{action=Index}/{id?}",
                constraints: new
                {
                    lang = "en|az|ru"
                });







                //+//
                // Scaffolding icindekileri burda yaziriq cagrilmasi ise ConfigureServices methodundadir(admin  areasi yaradanda olur)+
                cfg.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );


                cfg.MapControllerRoute("default", "{controller=home}/{action=index}/{id?}");
            });
        }
    }
}

// Dependency Injection bize komek eliyirki her defe DbContext inistansini(nusxesini) yaratmayaq ozu bize injent elesin;
// her bir contrelleri ozu bizim ucun gondersin,MVC elave edilib kim isdiyir elesin kim isdiyir elemesin,
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

