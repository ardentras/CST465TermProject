using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace final.Models
{
    public class SavedItem
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int Tab { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
    }
}
