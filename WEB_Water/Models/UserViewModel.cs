using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;

namespace WEB_Water.Models
{
    public class UserViewModel : User
    {
        [Display(Name ="Image")]
        public IFormFile ImageFile { get; set; }
    }
}
