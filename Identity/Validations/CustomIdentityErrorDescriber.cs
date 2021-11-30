using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Validations
{
    public class CustomIdentityErrorDescriber :IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = "DuplicateUserName",
                Description = $"Bu kullanıcı adı ({userName})kullanımaktadır. Başka bir kullanıcı adı seçiniz."
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = "InvalidUserName",
                Description = $"Bu kullanıcı adı ({userName}) geçersizdir."
            };
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = "DuplicateEmail",
                Description = $"Bu email adresi ({email}) kullanılmaktadır. Başka bir email adresi seçiniz."
            };
        }
        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = "InvalidEmail",
                Description = "Email adresiniz doğru formatta olmalıdır."
            };
        }
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = "PasswordTooShort",
                Description = $"Şifreniz en az {length} karakter olmalıdır."
            };
        }
    }
}
