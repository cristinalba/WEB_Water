using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Water.Data.Entities
{
    public class Reading
    {
        public int Id { get; set; }

        //public bool WasDeleted { get; set; }

        [Required]
        public Address AddressDetails { get; set; }

        [Display(Name = "Day of register")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime MonthlyReadingDate { get; set; }

        [Display(Name = "Monthly consume")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double MonthlyConsume { get; set; }

        [Display(Name = "Value to pay")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double ValueToPay { get; set; }

        [Display(Name = "Way of payment")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]//ComboBox
        public string WayOfPayment { get; set; }

        [Display(Name = "Status of payment")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]//ComboBox
        public string StatusOfPayment { get; set; }

    }
}
