using Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.ClaimsProvider
{
    public class ClaimProvider : IClaimsTransformation
    {
        public UserManager<AppUser> _userManager { get; set; }
        public ClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {

            if (principal != null && principal.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
                AppUser user = await _userManager.FindByNameAsync(identity.Name);
                if (user != null)
                {
                    if (!principal.HasClaim(c => c.Type == "city")&&user.City!=null)
                    {
                        Claim cityClaim = new Claim("city", user.City, ClaimValueTypes.String, "LOCAL  AUTHORİTY");

                        identity.AddClaim(cityClaim);
                    }
                }

                if (user!=null && user.BirthDay != null)
                {
                    var today = DateTime.Now.Year;
                    var age = today - user.BirthDay?.Year;
                    if (age > 15)
                    {
                        Claim violenceClaim = new Claim("violence", true.ToString(), ClaimValueTypes.String, "LOCAL  AUTHORİTY");
                        identity.AddClaim(violenceClaim);
                    }

                }

            }



            return principal;
        }
    }
}
