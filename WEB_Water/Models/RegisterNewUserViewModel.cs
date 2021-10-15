using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;

namespace WEB_Water.Models
{
    public class RegisterNewUserViewModel : User
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }   

        [MaxLength(20, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }

        //[Display(Name = "Image")]
        //public IFormFile ImageFile { get; set; }

        //[Display(Name = "Image")]
        //public Guid ImageId { get; set; }

    }
}
