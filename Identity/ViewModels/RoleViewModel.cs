﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.ViewModels
{
    public class RoleViewModel
    {
        [Display(Name ="Rol adı")]
        [Required(ErrorMessage ="Rol ismi gereklidir.")]
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
