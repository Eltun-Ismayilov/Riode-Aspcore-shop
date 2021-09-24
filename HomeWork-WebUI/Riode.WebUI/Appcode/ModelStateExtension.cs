using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode
{
    static public partial class Extension
    {
        //Create commands isvalid yoxlamaq ucun yazilib.
        static public bool ModelStateValid(this IActionContextAccessor action )
        {
            return action.ActionContext.ModelState.IsValid;
        }
    }
}
