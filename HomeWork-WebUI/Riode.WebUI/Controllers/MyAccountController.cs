﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Controllers
{
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
    }
}
