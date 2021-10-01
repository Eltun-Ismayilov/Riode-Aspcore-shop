using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Riode.WebUI.Appcode.Provider
{
    //Membership ucun yazilib SuperAdmin isdese kiminse rolnu ekinde ala biler hemin insan aktiv ola ola sebekeni kece biler.
    public class AppClaimProvider : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            throw new NotImplementedException();
        }
    }
}
