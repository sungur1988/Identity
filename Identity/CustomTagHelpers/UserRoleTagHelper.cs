using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.CustomTagHelpers
{
    [HtmlTargetElement("td", Attributes = "user-roles")]
    public class UserRoleTagHelper : TagHelper
    {
        public UserManager<AppUser> userManager { get; set; }
        public UserRoleTagHelper(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }


        [HtmlAttributeName("user-roles")]
        public string userId { get; set; }


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            AppUser appUser = await userManager.FindByIdAsync(userId);
            var userRoles = await userManager.GetRolesAsync(appUser) as List<string>;

            string html = String.Empty;
            if (userRoles.Count == 0)
            {
                html += "<span class='badge badge-danger'>Kullanıcıya ait herhangi bir rol toktur</span>";
            }
            else
            {
                foreach (var role in userRoles)
                {
                    html += $"<div><span class='badge badge-info'>{role}</span></div>";
                }
            }

            output.Content.SetHtmlContent(html);
        }
    }
}
