using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using final.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace final.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private RoleManager<IdentityRole> role_manager;
        private UserManager<finalUser> user_manager;
        private SiteSettings sitesettings;
        public AdminController(IOptions<SiteSettings> siteConfig, RoleManager<IdentityRole> roleManager, UserManager<finalUser> userManager)
        {

            role_manager = roleManager;
            user_manager = userManager;
            sitesettings = siteConfig.Value;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            ViewBag.SiteSettings = sitesettings;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(finalUser user)
        {
            if (!sitesettings.allowDeleteUser)
            {
                var temp = user_manager.DeleteAsync(user_manager.FindByIdAsync(user.Id).Result);
            }

            return RedirectToAction("Users");
        }

        public IActionResult Roles()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRole(IdentityRole role)
        {
            var temp = role_manager.CreateAsync(role).Result;

            return RedirectToAction("Roles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRole(IdentityRole role)
        {
            var temp = role_manager.DeleteAsync(role_manager.FindByIdAsync(role.Id).Result);

            return RedirectToAction("Roles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRole(IdentityRole role)
        {
            var temp = user_manager.AddToRoleAsync(user_manager.FindByNameAsync(role.NormalizedName).Result, role.Name).Result;

            return RedirectToAction("Roles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveRole(IdentityRole role)
        {
            var temp = user_manager.RemoveFromRoleAsync(user_manager.FindByNameAsync(role.NormalizedName).Result, role.Name).Result;

            return RedirectToAction("Roles");
        }
    }
}
