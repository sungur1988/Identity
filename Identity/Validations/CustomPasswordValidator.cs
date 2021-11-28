using Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Validations
{
    public class CustomPasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();
            if (password.ToLower().Contains(user.UserName))
            {
                if (!user.Email.ToLower().Contains(user.UserName))
                {
                    errors.Add(new IdentityError { Code = "PasswordContainUserName", Description="Şifre kullanıcı adı içermemelidir." });
                }
            }

            if (password.ToLower().Contains(user.Email))
            {
                errors.Add(new IdentityError { Code = "PasswordContainEmail", Description = "Şifre email adresi içermemelidir." });
            }
            for (int i = 1; i < password.Length-2; i++)
            {
                if (password[i]==password[i-1]&& password[i] == password[i + 1])
                {
                    errors.Add(new IdentityError { Code = "PasswordContainsConsecutiveCharacter", Description = "Şifre aynı karakteri üç defa ard arda içeremez." });
                }
            }
            if (errors.Count==0)
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
        }
    }
}
