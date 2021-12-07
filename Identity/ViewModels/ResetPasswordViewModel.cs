using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Email adresi boş olamaz.")]
        [Display(Name = "Email adresi: ")]
        [EmailAddress(ErrorMessage = "Email adresi doğru formatta olmalıdır.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş geçilemez.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifreniz: ")]
        public string PasswordNew { get; set; }
    }
}
