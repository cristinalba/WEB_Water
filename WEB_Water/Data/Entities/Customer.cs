using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Water.Data.Entities
{
    public class Customer
    {

        #region Propiedades
        public int Id { get; set; }

        //public bool WasDeleted { get; set; }

        [Required] //Mandatory
        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "NIF")]
        public int NIF_customer { get; set; }

        [Required]
        [Display(Name = "Telefone")]
        public int Telefone { get; set; }

        [Required]
        [Display(Name = "Email")]
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string Email { get; set; }

        //public User User { get; set; } //Once login is done, the user can control the Customers

        //public List<int> AddressID { get; set; }
        //Table con foreign key cliente

        //[Display(Name = "Full Name")]
        //public string FullName => $"{FirstName} {LastName}";

        #endregion
    
    }
}
