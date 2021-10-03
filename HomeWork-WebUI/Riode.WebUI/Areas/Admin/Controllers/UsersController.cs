using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Riode.WebUI.Model.DataContexts;
using Riode.WebUI.Model.Entity.Membership;
using Riode.WebUI.Model.Entity.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]


    public class UsersController : Controller
    {
        private readonly UserManager<RiodeUser> userManager;
        private readonly RiodeDbContext db;

        public UsersController(UserManager<RiodeUser> userManager, RiodeDbContext db)
        {

            this.userManager = userManager;
            this.db = db;

        }
        public async Task<IActionResult> Index()
        {
            List<RiodeUser> riodeUsers = userManager.Users.ToList();


            List<UserVM> users = new List<UserVM>();
            foreach (RiodeUser item in riodeUsers)
            {
                UserVM vm = new UserVM
                {
                    Id = item.Id,
                    Email = item.Email,
                    UserName = item.UserName,
                    Activated = item.Activates,
                    Role = ((await userManager.GetRolesAsync(item))[0])

                };
                users.Add(vm);
            }
            return View(users);

        }


        public async Task<IActionResult> Activated(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            RiodeUser user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();

            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activated(string id, bool Activated)
        {

            if (id == null)
            {
                return NotFound();
            }

            RiodeUser user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();

            }


            user.Activates = Activated;
            await db.SaveChangesAsync();
            return RedirectToAction("index");


        }


        public async Task<IActionResult> ChangRole(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            RiodeUser user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();

            }

            UserVM vm = new UserVM
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Activated = user.Activates,
                Role = ((await userManager.GetRolesAsync(user))[0]),
                Roles= new List<string> { "SuperAdmin","User"}
            };

            return null;
        }

    }
}
