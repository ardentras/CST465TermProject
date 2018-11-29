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
    public class NoteController : Controller
    {
        private INoteRepository note_repo;
        private SiteSettings sitesettings;

        public NoteController(IOptions<SiteSettings> siteConfig, INoteRepository noteRepository)
        {
            note_repo = noteRepository;
            sitesettings = siteConfig.Value;
        }

        [HttpPost]
        public IActionResult Edit(int id)
        {
            Note item = note_repo.GetItem(id);

            if (item == null)
            {
                item = new Note();
                item.Name = "ERROR: Item not found with that ID.";
                item.Text = "";
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Note item)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", item.ID);
            }

            if (item.Text == null)
            {
                item.Text = "";
            }
            note_repo.Update(item);

            return RedirectToAction("Index", "Vault");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (sitesettings.allowDeleteItem)
            {
                note_repo.Delete(id);
            }

            return RedirectToAction("Index", "Vault");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int id)
        {
            Note item = note_repo.GetItem(id);

            if (item == null)
            {
                item = new Note();
                item.Name = "ERROR: Item not found with that ID.";
                item.Text = "";
            }

            return View(item);
        }
    }
}
