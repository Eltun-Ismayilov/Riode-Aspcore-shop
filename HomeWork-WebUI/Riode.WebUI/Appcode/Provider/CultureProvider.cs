using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Provider
{

    //Multilang ucun RequestCultureProvider Toredirik.
    public class CultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            string lang = "az";

            string path = httpContext.Request.Path; // yuxardaki routdan biz ne yazildiqni tapiriq yeni (facabook.az)

            Match match = Regex.Match(path, @"\/(?<lang>az|en|ru)\/?.*"); // biz burda ise ne yazildiqni yoxluyuruq.

            //Link olaraq en/admin,az/admin yazib dili deyiwe bilir.

            if (match.Success)
            {
                lang = match.Groups["lang"].Value;

                httpContext.Response.Cookies.Delete("lang");
                httpContext.Response.Cookies.Append("lang", lang, new CookieOptions
                {

                    Expires = DateTime.Now.AddDays(3)
                });

                return Task.FromResult(new ProviderCultureResult(lang, lang));

            }

            // Burda ise evvelceden yadda saxlanildiqi dil ile girir

            if (httpContext.Request.Cookies.TryGetValue("lang",out lang))
            {
                return Task.FromResult(new ProviderCultureResult(lang, lang));

            }

            // 3 cu ise hecne yazilmiyibsa default olaraq 'az' olur.


            return Task.FromResult(new ProviderCultureResult(lang, lang));
        }
    }
}
