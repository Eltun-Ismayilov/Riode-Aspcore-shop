using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Riode.WebUI.Appcode;
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


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Wishlist()
        {
            return View();
        }

        readonly UserManager<RiodeUser> userManager;
        readonly SignInManager<RiodeUser> signInManager;
        public MyAccountController(UserManager<RiodeUser> userManager, SignInManager<RiodeUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        [AllowAnonymous]

        public IActionResult Singin()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Singin(LoginFormModel user)
        {

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

                if (!await userManager.IsInRoleAsync(founderUser,"User"))
                {
                    ViewBag.Ms = "Isdifadeci sifresi ve parol sefdir gagas";
                    return View(user);
                }

                var sRuselt = await signInManager.PasswordSignInAsync(founderUser, user.Password, true, true); //Burda giwi edirik.


                if (sRuselt.Succeeded != true) // Eger giriw zamani ugurlu deyilse yeni gire bilmirse 5
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
