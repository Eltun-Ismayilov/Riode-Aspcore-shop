using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Riode.WebUI.Appcode;
using Riode.WebUI.Appcode.Meddleware;
using Riode.WebUI.Appcode.Provider;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity.Membership;
using System;
using System.IO;
using System.Reflection;

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
                //SEKIL OXUMAQ UCUNDUR;
                cfg.ModelBinderProviders.Insert(0, new BooleanBinderProvider());


                //Membership ucun yazilb seyfeye giris edende login olmuyubsansa get login ol sonra gel 
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

                cfg.Filters.Add(new AuthorizeFilter(policy));


            })


                //+
                // productlari filter etmek ucundur loop olmasin
                .AddNewtonsoftJson(cfg =>
                {
                    cfg.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });



            //+
            // Patilarin Standart balaca herifnen yazilisi
            services.AddRouting(cfg => cfg.LowercaseUrls = true);




            //+
            // Dependency Injection Isdifade edilmesi ucun yazilmisdir
            services.AddDbContext<RiodeDbContext>(cfg =>
            {

                // ve burda cagirib yaziriq appsettings adini +
                cfg.UseSqlServer(configuration.GetConnectionString("cString"));

            }, ServiceLifetime.Scoped);

            //Mediatr ucun yazilib(arxekturani ucun)
            var currentAssembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(currentAssembly);



            //Membership ucun yazilmis kod(Datazbazaya bax)

            services.AddIdentity<RiodeUser, RiodeRole>()
                .AddEntityFrameworkStores<RiodeDbContext>();

            services.AddScoped<UserManager<RiodeUser>>(); // user idare etmek ucun menecer
            services.AddScoped<SignInManager<RiodeUser>>(); //giriw edende idare etmek ucun menecer;
            services.AddScoped<RoleManager<RiodeRole>>(); //role idare etmek ucun menecer;

            services.Configure<IdentityOptions>(cfg =>
            {

                cfg.Password.RequireDigit = false; //Reqem teleb elesin?
                cfg.Password.RequireUppercase = false; //Boyuk reqem teleb elesin?
                cfg.Password.RequireLowercase = false; //Kick reqem teleb elesin?
                cfg.Password.RequiredUniqueChars = 1; //Tekrarlanmiyan 1 sombol olsun?(11-22-3)
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

                cfg.ExpireTimeSpan = new TimeSpan(0, 5, 0);//Seni sayitda nece deq saxlasin eger sen hecne elemirsense atacaq yeni login olduqdan sonra diansan ve ya saty girdikden sonra diansan

                cfg.Cookie.Name = "Riode"; //Cookie adi ne olsun isdediyvi yazmalisan;
            });

            services.AddAuthentication(); //Sayita girmek demekdi(login olmaq)
            services.AddAuthorization(); // Sayitda ne ede bilmekdi yeni admin fikirlew;(Salahiyyetli olmaq)(siyahida gorunenleri create,delete ,update,edit elemekdir)
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //+
            // devoloper ucun Error cixarilmasi+
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //+
            app.UseRouting();

            //Membership ucundur ve burada yazilmalidir ruotin asagida useendpoint yuxarida;
            app.UseAuthentication();
            app.UseAuthorization();


            //MultiLang ucun yazilib
            app.UseRequestLocalization(cfg=> {
                cfg.AddSupportedUICultures("az", "en");
                cfg.AddSupportedCultures("az", "en");


                cfg.RequestCultureProviders.Clear();
                cfg.RequestCultureProviders.Add(new CultureProvider());

            }); 
            // static fayilarin oxunmasi ucun yazilmis kod+
            app.UseStaticFiles();

            //Meddleware cagrilma mentiqi bu curdur..
            app.UseAudit();



            app.UseEndpoints(cfg =>
            {

                cfg.MapGet("/coming-soon.html", async (context) =>
                {
                    using (var sr = new StreamReader("views/Static/coming-soon.html"))
                    {
                        context.Response.ContentType = "text/html";
                        await context.Response.WriteAsync(sr.ReadToEnd());
                    }

                });

                //Membersip ucun yazmisiq routda olanda myaccount/singin yox html kimi singin.html cixsin diye yaziriq;
                cfg.MapControllerRoute("x", "signin.html",
                  defaults:new
                  {
                   controller = "MyAccount",
                   action= "SingIn",
                   area=""
                  });


                //+
                // Scaffolding icindekileri burda yaziriq cagrilmasi ise ConfigureServices methodundadir
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

