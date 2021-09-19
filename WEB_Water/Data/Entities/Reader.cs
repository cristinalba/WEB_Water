using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Water.Data.Entities
{
    public class Reader : IEntity
    {
        public int Id { get; set; }

        //public bool WasDeleted { get; set; }

      
        //[Display(Name = "Reader Reference")]
        public string ReaderName { get; set; } 


        [Required]
        [Display(Name = "Customer")]
        public User User { get; set; }

        [Display(Name = "Address")]
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string AddressName { get; set; }


        [Display(Name = "Installation date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Installation { get; set; }


        //public List<int> ConsumosID { get; set; }
    }
}
