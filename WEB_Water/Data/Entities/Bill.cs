using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Water.Data.Entities
{
    public class Bill : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Customer")]
        public User User { get; set; }

        [Display(Name = "Water Reader")]
        public Reader Reader { get; set; }

        //Date of registering reading
        [Display(Name = "Invoice Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime BillDate { get; set; }


        [Display(Name = "To Pay")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double? ValueToPay { get; set; }


        public Reading Reading { get; set; }
    }
}
