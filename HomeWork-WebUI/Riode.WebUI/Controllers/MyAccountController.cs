using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Controllers
{
    [AllowAnonymous] //Memberhsip ucun yazilib biz icaze verdiyiz seyfe girsin.

    public class MyAccountController : Controller
    {

        // index
        public IActionResult Index()
        {
            return View();
        }
        //etrafli
        public IActionResult Wishlist()
        {
            return View();
        }
        //Login 
        public IActionResult SingIn()
        {
            return View();
        }
    }
}
