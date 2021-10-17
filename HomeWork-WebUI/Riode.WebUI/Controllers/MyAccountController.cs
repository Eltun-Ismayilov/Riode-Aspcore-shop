using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Riode.WebUI.Appcode;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity.FormModels;
using Riode.WebUI.Model.Entity.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Riode.WebUI.Controllers
{
    [AllowAnonymous] //Memberhsip ucun yazilib biz icaze verdiyiz seyfe girsin.

    public class MyAccountController : Controller
    {

        readonly UserManager<RiodeUser> userManager;
        readonly SignInManager<RiodeUser> signInManager;
        readonly RiodeDbContext db;
        readonly IConfiguration configuration;

        public MyAccountController(UserManager<RiodeUser> userManager, SignInManager<RiodeUser> signInManager, RiodeDbContext db, IConfiguration configuration)

        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.db = db;
            this.configuration = configuration;
        }

        [Route("/MyAccount.html")]

        public IActionResult Index()
        {
            return View();
        }
        [Route("/Wishlist.html")]

        public IActionResult Wishlist()
        {
            return View();
        }
        [AllowAnonymous]
        [Route("/Registir.html")]

        public IActionResult Registir()
        {
            //Eger giris edibse routda myaccount/sing yazanda o seyfe acilmasin homa tulaasin
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "Home");

            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/Registir.html")]

        public async Task<IActionResult> Registir(RegisterFormModel register)
        {
            //Eger giris edibse routda myaccount/sing yazanda o seyfe acilmasin homa tulaasin
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "Home");

            }
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            //Yeni user yaradiriq.
            RiodeUser user = new RiodeUser
            {

                UserName = register.UserName,
                Email = register.Email,
                Activates = true
            };


            string token = $"subscribetoken-{register.UserName}-{DateTime.Now:yyyyMMddHHmmss}"; // token yeni id goturuk

            token = token.Encrypt("");
                               //http vs https    fb.com       
            string path = $"{Request.Scheme}://{Request.Host}/subscribe-confirmm?token={token}"; // path duzeldirik



            var mailSended = configuration.SendEmail(user.Email, "Riode seyfesi gagas", $"Zehmet olmasa <a href={path}>Link</a> vasitesile abuneliyi tamamlayin");


            var person = await userManager.FindByNameAsync(user.UserName);


            if (person == null)
            {
                //Burda biz userManager vasitesile user ve RegistirVM passwword yoxluyuruq.(yaradiriq)
                var identityRuselt = await userManager.CreateAsync(user, register.Password);


                //Startupda yazdigimiz qanunlara uymursa Configure<IdentityOptions> onda error qaytariq summary ile.;
                if (!identityRuselt.Succeeded)
                {
                    foreach (var error in identityRuselt.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                //Yratdigimiz user ilk yarananda user rolu verik.

                await userManager.AddToRoleAsync(user, "User");

                return RedirectToAction("index", "Home");

            }


            if (person.UserName != null)
            {
                ViewBag.ms = "Bu username evvelceden qeydiyyatdan kecib";

                return View(register);
            }
            return null;
        }

        [HttpGet]
        [Route("subscribe-confirmm")]
        public IActionResult SubscibeConfirm(string token)
        {


            token = token.Decrypte("");

            Match match = Regex.Match(token, @"subscribetoken-(?<id>[a-zA-Z0-9]*)(.*)-(?<timeStampt>\d{14})");

            if (match.Success)
            {
                string id = match.Groups["id"].Value;
                string executeTimeStamp = match.Groups["executeTimeStamp"].Value;

                var subsc = db.Users.FirstOrDefault(s => s.UserName == id);

                if (subsc == null)
                {   
                    ViewBag.ms = Tuple.Create(true, "Token xetasi");
                    goto end;
                }
                if (subsc.EmailConfirmed == true)
                {
                    ViewBag.ms = Tuple.Create(true, "Artiq tesdiq edildi");
                    goto end;
                }
                subsc.EmailConfirmed = true;
                db.SaveChanges();

                ViewBag.ms = Tuple.Create(false, "Abuneliyiniz tesdiq edildi");

            }
            end:
            return View();
        }


        [AllowAnonymous]
        [Route("/Singin.html")]

        public IActionResult Singin()
        {

            //Eger giris edibse routda myaccount/sing yazanda o seyfe acilmasin homa tulaasin
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "Home");

            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/Singin.html")]

        public async Task<IActionResult> Singin(LoginFormModel user)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "Home");

            }

            if (ModelState.IsValid)
            {

                RiodeUser founderUser = null;

                if (user.UserName.IsEmail())
                {
                    founderUser = await userManager.FindByEmailAsync(user.UserName); //Eger Isdifadeci email gore giris edibse onu yoxlayir 
                }
                else
                {
                    founderUser = await userManager.FindByNameAsync(user.UserName); //YOX EGER USERNAME GORE GIRIBSE ONU AXTARIS EDIR.

                }

                if (founderUser == null) //Eger login ola bilmirse gonderir view gonderir yeni isdifadeci tapilmiyanda
                {
                    ViewBag.Ms = "Isdifadeci sifresi ve parol sefdir gagas";
                    return View(user);

                }

                if (founderUser.EmailConfirmed == false)
                {
                    ViewBag.Ms = "Zehmet olmasa Emailinizi testiq edin....";
                    return View(user);
                }

                //Eger database isdifadeci user deyilse onda gire bilmez.

                if (!await userManager.IsInRoleAsync(founderUser, "User"))
                {
                    ViewBag.Ms = "Isdifadeci sifresi ve parol sefdir gagas";
                    return View(user);
                }




                if (founderUser.Activates == false)
                {
                    ViewBag.ms = "Siz admin terefinden banlanmisiz";
                    return View(user);
                }

                if (founderUser.Activates == true)
                {
                    var sRuselt = await signInManager.PasswordSignInAsync(founderUser, user.Password, true, true); //Burda giwi edirik.

                    if (sRuselt.Succeeded != true) // Eger giriw zamani ugurlu deyilse yeni gire bilmirse 
                    {
                        ViewBag.Ms = "Isdifadeci sifresi ve parol sefdir gagas";
                        return View(user);

                    }
                    var redirectUrl = Request.Query["ReturnUrl"];

                    if (!string.IsNullOrWhiteSpace(redirectUrl))
                    {
                        return Redirect(redirectUrl);
                    }
                    return RedirectToAction("Index", "Home");

                }


                //eger giriw eden rolu nedise onu o area aparsin ya admin yada oz application
                //string role = (await userManager.GetRolesAsync(founderUser))[0];
                //if (role == "SuperAdmin")
                //{
                //    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                //}



                // Eger biz login olduqdan sonra isdediymiz yere getsin



                // yox eger bosdusa home aparsin
            }
            ViewBag.Ms = "Melumatlari doldur gagas";
            return View(user);
        }
        [Route("/Logout.html")]

        public async Task<IActionResult> Logout()
        {

            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Singin));

        }
    }
}
