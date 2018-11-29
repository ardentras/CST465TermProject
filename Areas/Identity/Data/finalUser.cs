using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace final.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the finalUser class
    public class finalUser : IdentityUser
    {
        public int UserHash { get; set; }
    }
}
