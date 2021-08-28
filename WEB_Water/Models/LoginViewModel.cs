using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Water.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]//format of an email xxx@xxx.xxx
        public string Username { get; set; }

        [Required]
        [MinLength(6)]//at least 6 digits
        public string Password { get; set; }


        public bool RememberMe { get; set; }
        //To Keep the user logged
    }
}
