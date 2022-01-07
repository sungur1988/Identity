using Identity.Filters;
using Identity.Helpers;
using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    public class HomeController : BaseController
    {
       

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : base(userManager,signInManager)
        {
            
        }

        public IActionResult LogIn(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
        {
            
                var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
                
                if (user != null)
                {
                    if (await _userManager.IsEmailConfirmedAsync(user)==false)
                    {
                        ModelState.AddModelError("", "Giriş yapabilmeniz için email adresiniz doğrulamanız gerekmektedir.Lütfen emailinizi kontrol ediniz.");
                        return View(loginViewModel);
                    }
                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("", "Hesabınız geçici süreliğine kilitlenmiştir. Lütfen daha sonra tekrar deneyiniz.");
                        return View(loginViewModel);
                    }
                    await _signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);
                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        if (TempData["ReturnUrl"]!=null)
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }
                        return RedirectToAction("Index", "Member");
                    }
                    else
                    {
                        await _userManager.AccessFailedAsync(user);
                        int fail = _userManager.GetAccessFailedCountAsync(user).Result;
                        ModelState.AddModelError("", $" {fail} kez başarısız giriş.");
                        if (fail==3)
                        {
                            await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(60)));
                            ModelState.AddModelError("", "Hesabınız 3 başarısız girişten dolayı 20 dakika süreyle kitlenmiştir. Lütfen daha sonra tekrar deneyiniz.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Email adresi veya şifreniz yanlış lütfen tekrar deneyiniz");
                        }
                    }

                }
           


            return View(loginViewModel);
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Member");
            }
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
            
                AppUser appUser = new AppUser();
                appUser.UserName = userViewModel.UserName;
                appUser.Email = userViewModel.Email;
                appUser.PhoneNumber = userViewModel.PhoneNumber;
                IdentityResult result = await _userManager.CreateAsync(appUser, userViewModel.Password);
                if (result.Succeeded)
                {
                    string emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                    string link = Url.Action("Confirm","Home",new { 
                    
                    userId= appUser.Id,
                    token=emailConfirmationToken
                    
                    },HttpContext.Request.Scheme);
                    Helpers.EmailConfirmation.SendEmail(link, appUser.Email);
                    return RedirectToAction("Login");
                }
                else
                {
                    AddModelError(result);
                }

            
            return View(userViewModel);
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword([Bind("Email")]ResetPasswordViewModel resetPasswordViewModel)
        {
            var user = _userManager.FindByEmailAsync(resetPasswordViewModel.Email).Result;
            if (user!= null)
            {
               
                string passwordResetToken = _userManager.GeneratePasswordResetTokenAsync(user).Result;

                string passwordResetLink = Url.Action("ResetPasswordConfirm", "Home" , new
                {
                    userId = user.Id,
                    token = passwordResetToken
                }, HttpContext.Request.Scheme);

                PasswordResetHelper.SendEmail(passwordResetLink, user.Email);
                ViewBag.status = "success";
            }
            else
            {
                ModelState.AddModelError("", "Belirtilen email adresine ait herhangi bir kullanıcı bulunamamıştır.");
            }
            return View(resetPasswordViewModel);
        }
        public IActionResult ResetPasswordConfirm(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm([Bind("PasswordNew")]ResetPasswordViewModel resetPasswordViewModel)
        {
            var user = await _userManager.FindByIdAsync(TempData["userId"].ToString());
            if (user!=null)
            {
              var result = await _userManager.ResetPasswordAsync(user, TempData["token"].ToString(), resetPasswordViewModel.PasswordNew);
                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                    ViewBag.status = "success";

                }
                else
                {
                    AddModelError(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Sistemsel bir hata meydana geldi lütfen tekrar deneyiniz.");
            }
            return View(resetPasswordViewModel);
            
        }
    }
}
