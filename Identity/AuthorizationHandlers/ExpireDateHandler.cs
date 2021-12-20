using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.AuthorizationHandlers
{
    public class ExpireDateHandler : AuthorizationHandler<ExpireDateRequirements>
    {
        protected override  Task HandleRequirementAsync(AuthorizationHandlerContext context, ExpireDateRequirements requirement)
        {
            if (context.User != null && context.User.Identity!=null)
            {
                var claim = context.User.Claims.Where(c => c.Type == "ExpireDate" && c.Value != null).FirstOrDefault();
                if (claim != null)
                {
                    if (DateTime.Now<Convert.ToDateTime(claim.Value))
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                }
            }
            return Task.CompletedTask;
        }


    }
}
