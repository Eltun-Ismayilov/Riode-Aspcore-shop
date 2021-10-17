using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Areas.Admin.Controllers
{
  //[AllowAnonymous]
    [Authorize(Roles = "SuperAdmin,Admin")]
    [Area("Admin")]
    public class DashboardController:Controller
    {
        public IActionResult Index()
        {
            return View();

        }

        public IActionResult Tables()
        {
            return View();

        }
        public IActionResult MailBox()
        {
            return View();

        }
        public IActionResult Layouts()
        {
            return View();

        }
        public IActionResult Chat()
        {
            return View();

        }

        public IActionResult Invoice()
        {
            return View();

        }

    }
}
