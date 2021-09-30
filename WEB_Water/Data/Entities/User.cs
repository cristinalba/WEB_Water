using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEB_Water.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string LastName { get; set; }

        [Display(Name = "NIF")]
        public string Nif { get; set; }

        [Display(Name = "Is Customer?")]
        public bool IsCustomer { get; set; }

        //[Display(Name = "Image")]
        //public string ImageUrl { get; set; }

        //public string ImageFullPath
        //{
        //    get
        //    {
        //        if(string.IsNullOrEmpty(ImageUrl))
        //        {
        //            return null;
        //        }
        //        return $"https://localhost:44333{ImageUrl.Substring(1)}"; //URL completa para ver la imagen
        //    }
        //}

        [Display(Name = "Customer")]
        public string FullName { get { return $"{this.FirstName} {this.LastName}"; } }

        public List<Reader> Readers { get; set; }
    }
}
