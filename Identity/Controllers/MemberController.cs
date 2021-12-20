using Identity.Models;
using Identity.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Identity.Enums;
using System.Security.Claims;

namespace Identity.Controllers
{
    [Authorize]
    public class MemberController : BaseController
    {
       

        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : base(userManager,signInManager)
        {
           
        }
        public IActionResult Index()
        {
            AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            UserViewModel userViewModel = user.Adapt<UserViewModel>();
            return View(userViewModel);
        }

        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
                bool exist = _userManager.CheckPasswordAsync(appUser, passwordChangeViewModel.PasswordOld).Result;
                if (exist)
                {
                    var result = _userManager.ChangePasswordAsync(appUser, passwordChangeViewModel.PasswordOld, passwordChangeViewModel.PasswordNew).Result;
                    if (result.Succeeded)
                    {
                        await _userManager.UpdateSecurityStampAsync(appUser);
                        await _signInManager.SignOutAsync();
                        await _signInManager.PasswordSignInAsync(appUser, passwordChangeViewModel.PasswordNew, true, false);
                        ViewBag.success = "true";

                    }
                    else
                    {
                        AddModelError(result);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Girdiğiniz eski şifreniz yanlıştır.");
                }

            }
            return View(passwordChangeViewModel);
        }


        public IActionResult UserEdit()
        {
            AppUser user = CurrentUser;
            UserViewModel userViewModel = user.Adapt<UserViewModel>();
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
            return View(userViewModel);

        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserViewModel userViewModel,IFormFile userPicture)
        {
            ModelState.Remove("Password");
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
            if (ModelState.IsValid)
            {
                AppUser user = CurrentUser;
                if (userPicture != null && userPicture.Length>0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userPicture.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/UserPictures", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await userPicture.CopyToAsync(stream);
                        user.Picture = "/UserPictures/"+fileName;

                    }
                }




                user.UserName = userViewModel.UserName;
                user.PhoneNumber = userViewModel.PhoneNumber;
                user.Email = userViewModel.Email;
                user.BirthDay = userViewModel.BirthDay;
                user.Gender = (int)userViewModel.Gender;
                user.City = userViewModel.City;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, true);
                    ViewBag.success = "true";
                }
                else
                {
                    AddModelError(result);
                }

            }
            return View(userViewModel);

        }

        public async void LogOut()
        {
            await _signInManager.SignOutAsync();
        }
        public IActionResult AccessDenied(string ReturnUrl)
        {
            if (ReturnUrl== "/Member/Adana")
            {
                ViewBag.message = "Bu sayfaya sadece adanalı kullanıcılar erişebilir.";
            }
            else if (ReturnUrl== "/Member/Violence")
            {
                ViewBag.message = "Bu sayfada şiddet içerikli videolar bulunduğundan dolayı sadece 15 yaşından büyükler erişebilir";
            }
            else if (ReturnUrl == "/Member/BorsaPage")
            {
                ViewBag.message = "Bu sayfaya ücretsiz erişim süreniz dolmuştur.Üyeliklerimize göz atabilirsiniz.";
            }
            else
            {
                ViewBag.message = "Bu sayfaya erişim izniniz yoktur. Erişim izni almak için site yöneticisiyle görüşünüz";
            }
            return View();
        }

        [Authorize(Roles ="Editör,Admin")]
        public IActionResult Editör()
        {
            return View();
        }
        [Authorize(Roles = "Manager,Admin")]
        public IActionResult Manager()
        {
            return View();
        }

        [Authorize(Policy ="CityPolicy")]
        public IActionResult Adana()
        {
            return View();
        }

        [Authorize(Policy = "ViolencePolicy")]
        public IActionResult Violence()
        {
            return View();
        }


        public async Task<IActionResult> RedirectBorsa()
        {
            bool result = User.HasClaim(c => c.Type == "ExpireDate");
            if (!result)
            {
                Claim expireClaim = new Claim("ExpireDate", DateTime.Now.AddDays(30).ToShortDateString(), ClaimValueTypes.String, "LocalAuthority");
                await _userManager.AddClaimAsync(CurrentUser, expireClaim);
                await _signInManager.SignOutAsync();

                await _signInManager.SignInAsync(CurrentUser, true);
            }
            return RedirectToAction("BorsaPage");
        }
        [Authorize(Policy = "ExpireDatePolicy")]
        public IActionResult BorsaPage()
        {
            return View();
        }
    }
}
