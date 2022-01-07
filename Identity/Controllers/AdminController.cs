using Identity.Models;
using Identity.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        public AdminController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) : base(userManager, null, roleManager)
        {

        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Users()
        {
            return View(_userManager.Users.ToList());
        }
        public IActionResult Roles()
        {
            return View(_rolemanager.Roles.ToList());
        }

        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleViewModel roleViewModel)
        {
            
                AppRole appRole = new AppRole();
                appRole.Name = roleViewModel.Name;
                var result = await _rolemanager.CreateAsync(appRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    AddModelError(result);
                }

            

            return View(roleViewModel);
        }

        public IActionResult RoleUpdate(string id)
        {
            AppRole appRole = _rolemanager.FindByIdAsync(id).Result;
            if (appRole != null)
            {
                return View(appRole.Adapt<RoleViewModel>());
            }
            return RedirectToAction("Roles");
        }

        [HttpPost]
        public IActionResult RoleUpdate(RoleViewModel roleViewModel)
        {
            AppRole appRole = _rolemanager.FindByIdAsync(roleViewModel.Id).Result;
            if (appRole != null)
            {
                appRole.Name = roleViewModel.Name;
                var result = _rolemanager.UpdateAsync(appRole).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    AddModelError(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Sistemsel bir hata oluştu.Lütfen roller sayfasına geri dönün.");

            }

            return View(roleViewModel);
        }

        public IActionResult RoleDelete(string id)
        {
            AppRole appRole = _rolemanager.FindByIdAsync(id).Result;
            if (appRole != null)
            {
                var result = _rolemanager.DeleteAsync(appRole).Result;

            }
            return RedirectToAction("Roles");
        }

        public IActionResult RoleAssign(string id)
        {
            TempData["userId"] = id;

            AppUser appUser = _userManager.FindByIdAsync(id).Result;

            var userRoles = _userManager.GetRolesAsync(appUser).Result;
            ViewBag.userName = appUser.UserName;

            List<AppRole> appRoles = _rolemanager.Roles.ToList();
            List<RoleAssignViewModel> roleAssignViews = new List<RoleAssignViewModel>();

            foreach (var role in appRoles)
            {
                RoleAssignViewModel roleAssign = new RoleAssignViewModel();
                roleAssign.RoleName = role.Name;
                roleAssign.RoleId = role.Id;
                if (userRoles.Contains(role.Name))
                {
                    roleAssign.Exist = true;
                }
                else
                {
                    roleAssign.Exist = false;
                }
                roleAssignViews.Add(roleAssign);
            }


            return View(roleAssignViews);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> roleAssignViewModels)
        {
            AppUser appUser = await _userManager.FindByIdAsync(TempData["userId"].ToString());

            foreach (var role in roleAssignViewModels)
            {
                if (role.Exist)
                {
                    await _userManager.AddToRoleAsync(appUser, role.RoleName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(appUser, role.RoleName);
                }
            }





            return RedirectToAction("Users");
        }


        public IActionResult Claims()
        {
            return View(User.Claims.ToList());
        }


    }
}
