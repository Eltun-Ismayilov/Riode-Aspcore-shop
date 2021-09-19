using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Riode.WebUI.Appcode;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Riode.WebUI.Controllers
{
    public class HomeController : Controller
    {
        readonly RiodeDbContext db;

        readonly IConfiguration configuration;
        public HomeController(RiodeDbContext db, IConfiguration configuration)
        {
            this.db = db;
            this.configuration = configuration;

        }

        // Layout+
        public IActionResult Index()
        {
            return View();
        }
        // contact+
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactPost model)
        {

            if (ModelState.IsValid)
            {
                db.Add(model);

                db.SaveChanges();



                return Json(new
                {
                    // error yoxdusa bura dusur
                    error = false,
                    message = "Sorgunuz qeyde alindir"
                });


            }
            return Json(new
            {

                // error varsa bura dusur
                error = true,
                message = "Mellumatin dogrulugnu yoxluyun"
            });

        }
        // about+
        public IActionResult AboutUS()
        {
            return View();
        }
        // Fak+
        public IActionResult Faq()
        {

            var questions = db.Questions
             .Where(a => a.DeleteByUserId == null)
             .ToList();


            return View(questions);
        }


        [HttpPost]
        public IActionResult Subscrice([Bind("Email")] Subscrice model)
        {

            if (ModelState.IsValid)
            {
                var current = db.Subscrices.FirstOrDefault(s => s.Email.Equals(model.Email));
                if (current != null && current.EmailConfirmed == true)
                {
                    return Json(new
                    {

                        error = true,
                        massege = "Bu E-Mail evvelceden qeydiyyati edilib"

                    });
                }
                else if (current != null && (current.EmailConfirmed ?? false == false))
                {
                    return Json(new
                    {

                        error = true,
                        massege = "Bu E-Mail evvelceden qeydiyyati edilib "

                    });
                }


                db.Subscrices.Add(model);
                db.SaveChanges();


                string token = $"subscribetoken-{model.Id}-{DateTime.Now:yyyyMMddHHmmss}"; // token yeni id goturuk

                token = token.Encrypt("");

                string path = $"{Request.Scheme}://{Request.Host}/subscribe-confirm?token={token}"; // path duzeldirik



                var mailSended = configuration.SendEmail(model.Email, "Riode seyfesi gagas", $"Zehmet olmasa <a href={path}>Link</a> vasitesile abuneliyi tamamlayin");
                if (mailSended == null)
                {
                    db.Database.RollbackTransaction();

                    return Json(new
                    {
                        error = false,
                        massege = "Email-gonderilmasi zamini xeta bas verdi!"

                    });
                }

                return Json(new
                {

                    error = false,
                    massege = "Sorgunuz ugurla qeyde alindi!!"

                });
            }

            return Json(new
            {

                error = true,
                massege = "Sorgunuzun Icrasi zamani xeta yarandi,Zehmet olmasa yeniden yoxlayin"

            });
        }

        [HttpGet]
        [Route("subscribe-confirm")]
        public IActionResult SubscibeConfirm(string token)
        {


            token = token.Decrypte("");
            Match match = Regex.Match(token, @"subscribetoken-(?<id>\d)-(?<executeTimeStamp>\d{14})");

            if (match.Success)
            {
                int id = Convert.ToInt32(match.Groups["id"].Value);
                string executeTimeStamp = match.Groups["executeTimeStamp"].Value;

                var subsc = db.Subscrices.FirstOrDefault(s => s.Id == id);

                if (subsc == null)
                {
                    ViewBag.ms = Tuple.Create(true, "Token xetasi");
                    goto end;
                }
                if ((subsc.EmailConfirmed ?? false) == true)
                {
                    ViewBag.ms = Tuple.Create(true, "Artiq tesdiq edildi");
                    goto end;
                }
                subsc.EmailConfirmed = true;
                subsc.EmailConfirmedDate = DateTime.Now;
                db.SaveChanges();

                ViewBag.ms = Tuple.Create(false, "Abuneliyiniz tesdiq edildi");

            }
            end:
            return View();
        }

    }
}
