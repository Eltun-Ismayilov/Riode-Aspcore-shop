using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode
{
    static public partial class Extension
    {
        static public string GetCurrentCulture(this HttpContext ctx)
        {
            Match match = Regex.Match(ctx.Request.Path, @"\/(?<lang>az|en|ru)\/?.*"); // biz burda ise ne yazildiqni yoxluyuruq.

            if (match.Success)
                return match.Groups["lang"].Value;
            if (ctx.Request.Cookies.TryGetValue("lang", out string lang))

                return lang;


            return "az";

        }
    }
}
