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
    public class WebsiteController : Controller
    {
        private IWebsiteRepository website_repo;
        private SiteSettings sitesettings;

        public WebsiteController(IOptions<SiteSettings> siteConfig, IWebsiteRepository websiteRepository)
        {
            website_repo = websiteRepository;
            sitesettings = siteConfig.Value;
        }

        [HttpPost]
        public IActionResult Edit(int id)
        {
            Website item = website_repo.GetItem(id);

            if (item == null)
            {
                item = new Website();
                item.Name = "ERROR: Item not found with that ID.";
                item.Username = "";
                item.URL = "";
                item.PasswordValue = "";
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Website item)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", item.ID);
            }

            if (item.Username == null)
            {
                item.Username = "";
            }
            if (item.URL == null)
            {
                item.URL = "";
            }
            if (item.PasswordValue == null)
            {
                item.PasswordValue = "";
            }
            website_repo.Update(item);

            return RedirectToAction("Index", "Vault");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (sitesettings.allowDeleteItem)
            {
                website_repo.Delete(id);
            }
            return RedirectToAction("Index", "Vault");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int id)
        {
            Website item = website_repo.GetItem(id);

            if (item == null)
            {
                item = new Website();
                item.Name = "ERROR: Item not found with that ID.";
                item.Username = "";
                item.URL = "";
                item.PasswordValue = "";
            }

            return View(item);
        }
    }
}
