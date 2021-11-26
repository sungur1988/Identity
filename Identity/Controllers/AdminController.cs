using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Identity.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<AppUser> _manager { get; }
        public AdminController(UserManager<AppUser> manager)
        {
            _manager = manager;
        }
        public IActionResult Index()
        {
            var users = _manager.Users.ToList();
            return View(users);
        }
    }
}
