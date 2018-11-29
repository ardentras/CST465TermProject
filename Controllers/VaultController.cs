using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using final.Areas.Identity.Data;
using final.Models;
using final.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace final.Controllers
{
    [Authorize]
    public class VaultController : Controller
    {
        private INoteRepository note_repo;
        private IPasswordRepository password_repo;
        private IWebsiteRepository website_repo;
        private UserManager<finalUser> user_manager;
        private SiteSettings sitesettings;

        public VaultController(IOptions<SiteSettings> siteConfig, UserManager<finalUser> userManager, INoteRepository noteRepository, IPasswordRepository passwordRepository, IWebsiteRepository websiteRepository)
        {
            note_repo = noteRepository;
            password_repo = passwordRepository;
            website_repo = websiteRepository;
            user_manager = userManager;
            sitesettings = siteConfig.Value;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.SiteSettings = sitesettings;

            List<SavedItem> saved = new List<SavedItem>();

            finalUser user = user_manager.FindByNameAsync(User.Identity.Name).Result;
            foreach (var item in note_repo.GetList())
            {
                if (item != null)
                {
                    if (item.UserID == user.UserHash)
                    {
                        saved.Add(item);
                    }
                }
            }
            foreach (var item in password_repo.GetList())
            {
                if (item.UserID == user.UserHash)
                {
                    saved.Add(item);
                }
            }
            foreach (var item in website_repo.GetList())
            {
                if (item.UserID == user.UserHash)
                {
                    saved.Add(item);
                }
            }

            return View(saved);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetNote()
        {
            return PartialView("Partials/_NoteFormPartial");
        }

        [HttpGet]
        public IActionResult GetPassword()
        {
            return PartialView("Partials/_PasswordFormPartial");
        }

        [HttpGet]
        public IActionResult GetWebsite()
        {
            return PartialView("Partials/_WebsiteFormPartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNote(Note item)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", item);
            }

            item.UserID = user_manager.FindByNameAsync(User.Identity.Name).Result.UserHash;
            if (item.Text == null)
            {
                item.Text = "";
            }

            note_repo.Insert(item);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePassword(Password item)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", item);
            }

            item.UserID = user_manager.FindByNameAsync(User.Identity.Name).Result.UserHash;

            if (item.Username == null)
            {
                item.Username = "";
            }
            if (item.PasswordValue == null)
            {
                item.PasswordValue = "";
            }

            password_repo.Insert(item);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateWebsite(Website item)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", item);
            }

            item.UserID = user_manager.FindByNameAsync(User.Identity.Name).Result.UserHash;

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

            website_repo.Insert(item);

            return RedirectToAction("Index");
        }
    }
}
