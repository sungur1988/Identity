using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.ViewModels
{
    public class PasswordChangeViewModel
    {
        [Display(Name ="Eski Şifreniz")]
        [Required(ErrorMessage ="Eski şifreniz gereklidir.")]
        [DataType(DataType.Password)]
        [MinLength(4)]
        public string PasswordOld { get; set; }

        [Display(Name = "Yeni Şifreniz")]
        [Required(ErrorMessage = "Yeni şifreniz gereklidir.")]
        [DataType(DataType.Password)]
        [MinLength(4)]
        public string PasswordNew { get; set; }


        [Display(Name = "Onay Yeni Şifreniz")]
        [Required(ErrorMessage = "Onay Yeni şifreniz gereklidir.")]
        [DataType(DataType.Password)]
        [MinLength(4)]
        public string PasswordConfirm { get; set; }
    }
}
