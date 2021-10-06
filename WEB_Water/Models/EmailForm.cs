using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Water.Models
{
    public class EmailForm
    {
        //Contact Form
        //[Required(ErrorMessage = "'Name' required")]
        //public string Name { get; set; }

        //[Required(ErrorMessage = "'Mail' required")]
        //public string Email { get; set; }

        //[Required(ErrorMessage = "'Subject' required")]
        //public string Subject { get; set; }

        //[Required(ErrorMessage = "'Message' required")]
        //public string Message { get; set; }

        //New Reader Form
        [Required(ErrorMessage = "'First Name' required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "'Last Name' required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "'Mail' required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "'NIF' required")]
        public string NIF { get; set; }

        [Required(ErrorMessage = "'Telephone' required")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "'Address' required")]
        public string Address { get; set; }




    }
}
