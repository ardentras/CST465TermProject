using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace final.Models
{
    public class Note : SavedItem
    {
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
    }
}
