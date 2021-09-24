using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Provider
{

    //Multilang ucun RequestCultureProvider Toredirik.
    public class CultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            string lang = "en";
            return Task.FromResult(new ProviderCultureResult(lang, lang));
        }
    }
}
