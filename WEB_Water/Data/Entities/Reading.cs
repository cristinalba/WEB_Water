using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Water.Data.Entities
{
    public class Reading : IEntity
    {
        public int Id { get; set; }

        //public bool WasDeleted { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public User User { get; set; }

        [Required]
        [Display(Name = "Reader")]
        public Reader Reader { get; set; }

        //Período
        [Display(Name = "Begin date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Begin { get; set; }

        //Período
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? End { get; set; } 




        [Display(Name = "Monthly consume (m³)")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double MonthlyConsume { get; set; }

        [Display(Name = "Invoice issued")]
        public bool BillIssued { get; set; }



    }
}
