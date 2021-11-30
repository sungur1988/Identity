using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email adresi boş olamaz.")]
        [Display(Name ="Email adresi")]
        [EmailAddress(ErrorMessage = "Email formatı yanlış")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Şifre alanı boş geçilemez.")]
        [Display(Name ="Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
