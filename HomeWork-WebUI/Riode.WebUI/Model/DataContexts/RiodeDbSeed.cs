using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Riode.WebUI.Model.Entity.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.DataContexts
{
    public static class RiodeDbSeed
    {
        public static IApplicationBuilder SeedMembership(this IApplicationBuilder builder)
        {

            using (var scope=builder.ApplicationServices.CreateScope())
            {

                //Role hisse burda yazilicaq.
                var role = new RiodeRole {    // Database SuperAdmin tipinde Role yaratdiq.
                    Name="SuperAdmin"           
                };



                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RiodeRole>>();  //Role idare etmek ucun RoleManager isdifade olunur.

                bool hasRole = roleManager.RoleExistsAsync(role.Name).Result;//Yoxluyuruq eger database Role(SuperAdmin varsa) //Ona gore yaziriqki eger Async varsa method asnyc yazilmalidir onu yazmaqdansa Result yazilir..

                if (hasRole==true)
                {
                    role = roleManager.FindByNameAsync(role.Name).Result; //Eger superadmin dababase varsa onan isdifade edeciyik(Yeni o Rolu isdifade edeceyik.)
                }
                else
                {
                   var IResult=roleManager.CreateAsync(role).Result; //Eger database yoxdusa Yeni Superadmin rolu yaradacq.

                    if (!IResult.Succeeded)  //Eger yenisini Yarada bilmiyibse.

                        goto end;
                }
                //User hisse burda yazilicaq.

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<RiodeUser>>();

                string password = "123";

                // Yeni user yaradiriq.
                var user = new RiodeUser { 
                
                    UserName="eltun",
                    Email= "eltun@mail.ru",
                    EmailConfirmed=true
                };
                //FindByEmailAsnyc bize tapmiyanda null tapanda ise null qaytarmir ve tapmiyanda user icindekilerni null edir.
                var foundedUser = userManager.FindByEmailAsync(user.Email).Result; //Database user(eltun,eltun@mail.ru) Adli melumat var axtariq.

                //Demeli User var. axtariw neticesinde tapilib.(57)
                if (foundedUser!=null && !userManager.IsInRoleAsync(foundedUser, role.Name).Result)
                {

                   // userManager.IsInRoleAsync(foundedUser, role.Name).Result;//Eger User bu roldadisa hecne eleme.

                    userManager.AddToRoleAsync(foundedUser, role.Name).Wait(); //Burda biz yaratdiqmiz user rola atiriq.
                }
                else if (foundedUser == null)
                {
                    var IUserResult=userManager.CreateAsync(user, password).Result; // yox eger database hemin user yoxdusa yenisini yaratsin.

                    if (IUserResult.Succeeded) //uGURLUDUSA 
                    {
                        userManager.AddToRoleAsync(user, role.Name).Wait(); //Burda biz yaratdiqmiz user rola atiriq.

                    }
                }
            }
            end:
            return builder;

        }
    }
}
