using Identity.Models;
using Identity.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Controllers
{
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
            if (ModelState.IsValid)
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
    }
}
