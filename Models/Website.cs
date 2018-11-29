﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace final.Models
{
    public class Website : SavedItem
    {
        public string Username { get; set; }

        public string URL { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string PasswordValue { get; set; }
    }
}
