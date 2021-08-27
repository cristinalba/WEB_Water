using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Water.Data.Entities
{
    public class Address : IEntity
    {
        public int Id { get; set; }

        //public bool WasDeleted { get; set; }

        [Required]
        public Customer Customer { get; set; }

        [Display(Name = "Address")]
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string AddressName { get; set; }

        [Display(Name = "Postal Code")]
        [MaxLength(9, ErrorMessage = "The format must be numbers XXXX-XXX.")]
        public string PostalCode { get; set; }

        [Display(Name = "City")]
        [MaxLength(70, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string City { get; set; }


        [Display(Name = "Begin of contract")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime BeginOfContract { get; set; }

        [Display(Name = "Name of Bank")]
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string NameOfBank { get; set; }

        //public List<int> ConsumosID { get; set; }
    }
}
