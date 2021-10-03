using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Riode.WebUI.Appcode;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity.FormModels;
using Riode.WebUI.Model.Entity.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Controllers
{
    [AllowAnonymous] //Memberhsip ucun yazilib biz icaze verdiyiz seyfe girsin.

    public class MyAccountController : Controller
    {

        readonly UserManager<RiodeUser> userManager;
        readonly SignInManager<RiodeUser> signInManager;
        readonly RiodeDbContext db;
        public MyAccountController(UserManager<RiodeUser> userManager, SignInManager<RiodeUser> signInManager, RiodeDbContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.db = db;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Wishlist()
        {
            return View();
        }
        [AllowAnonymous]

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
        public async Task<IActionResult> Registir(RegisterFormModel register)
        {
            //Eger giris edibse routda myaccount/sing yazanda o seyfe acilmasin homa tulaasin
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "Home");

            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            //Yeni user yaradiriq.
            RiodeUser user = new RiodeUser
            {

                UserName = register.UserName,
                Email = register.Email,
                EmailConfirmed = true


            };

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


        [AllowAnonymous]

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

                //Eger database isdifadeci user deyilse onda gire bilmez.

                if (!await userManager.IsInRoleAsync(founderUser, "User"))
                {
                    ViewBag.Ms = "Isdifadeci sifresi ve parol sefdir gagas";
                    return View(user);
                }

                var sRuselt = await signInManager.PasswordSignInAsync(founderUser, user.Password, true, true); //Burda giwi edirik.

                //eger giriw eden rolu nedise onu o area aparsin ya admin yada oz application
                //string role = (await userManager.GetRolesAsync(founderUser))[0];
                //if (role == "SuperAdmin")
                //{
                //    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                //}

                if (sRuselt.Succeeded != true) // Eger giriw zamani ugurlu deyilse yeni gire bilmirse 
                {
                    ViewBag.Ms = "Isdifadeci sifresi ve parol sefdir gagas";
                    return View(user);

                }

                // Eger biz login olduqdan sonra isdediymiz yere getsin
                var redirectUrl = Request.Query["ReturnUrl"];

                if (!string.IsNullOrWhiteSpace(redirectUrl))
                {
                    return Redirect(redirectUrl);
                }


                // yox eger bosdusa home aparsin
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Ms = "Melumatlari doldur gagas";
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {

            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Singin));

        }
    }
}
