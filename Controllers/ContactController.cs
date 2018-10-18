using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CST465TermProject.Controllers
{
    public class ContactController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(string Name, string PhoneNumber, string Email, string Message)
        {
            Models.ContactModel model = new Models.ContactModel();

            model.Name = Name;
            model.PhoneNumber = PhoneNumber;
            model.Email = Email;
            model.Message = Message;

            return View("Contact", model);
        }
    }
}
