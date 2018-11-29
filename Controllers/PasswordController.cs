using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using final.Models;
using final.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace final.Controllers
{
    public class PasswordController : Controller
    {
        private IPasswordRepository password_repo;
        private SiteSettings sitesettings;

        public PasswordController(IOptions<SiteSettings> siteConfig, IPasswordRepository passwordRepository)
        {
            password_repo = passwordRepository;
            sitesettings = siteConfig.Value;
        }

        [HttpPost]
        public IActionResult Edit(int id)
        {
            Password item = password_repo.GetItem(id);

            if (item == null)
            {
                item = new Password();
                item.Name = "ERROR: Item not found with that ID.";
                item.Username = "";
                item.PasswordValue = "";
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Password item)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", item.ID);
            }

            if (item.Username == null)
            {
                item.Username = "";
            }
            if (item.PasswordValue == null)
            {
                item.PasswordValue = "";
            }
            password_repo.Update(item);

            return RedirectToAction("Index", "Vault");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (sitesettings.allowDeleteItem)
            {
                password_repo.Delete(id);
            }

            return RedirectToAction("Index", "Vault");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int id)
        {
            Password item = password_repo.GetItem(id);

            if (item == null)
            {
                item = new Password();
                item.Name = "ERROR: Item not found with that ID.";
                item.Username = "";
                item.PasswordValue = "";
            }

            return View(item);
        }
    }
}
