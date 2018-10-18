using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CST465TermProject.Models
{
    public class ContactModel
    {
        [Required(ErrorMessage = "Please enter a name")]
        [DisplayName("Enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter a phone number")]
        [DisplayName("Enter Phone Number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter an email")]
        [DisplayName("Enter Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a message")]
        [DisplayName("Enter a Message")]
        public string Message { get; set; }
    }
}
