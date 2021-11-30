using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage ="Kullanıcı adı boş olamaz.")]
        [Display(Name = "Kullanıcı Adı: ")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Email adresi boş olamaz.")]
        [Display(Name ="Email adresi: ")]
        [EmailAddress(ErrorMessage ="Email adresi doğru formatta olmalıdır.")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Şifre alanı boş geçilemez.")]
        [DataType(DataType.Password)]
        [Display(Name ="Şifreniz: ")]
        public string Password { get; set; }

        [Display(Name ="Tel No:")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
