using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.ViewModels
{
    public class RoleAssignViewModel
    {
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public bool Exist { get; set; }
    }
}
