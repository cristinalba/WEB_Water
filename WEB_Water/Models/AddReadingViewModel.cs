using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;

namespace WEB_Water.Models
{
    public class AddReadingViewModel
    {

        [Display(Name = "Customer")]
        public string UserId { get; set; }

        [Display(Name = "Reader")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a reader.")] 
        public int ReaderId { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "Value must be a positive number")]
        [Display(Name = "Value in m³")]
        [DisplayFormat(DataFormatString = "{0:N3}")]
        public double ValueOfConsume { get; set; }


        [Display(Name = "Start")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Start { get; set; }

        [Display(Name = "End")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? End { get; set; }
       
        [Display(Name = "Registration date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime RegistrationDateNewReading { get; set; }

        public User User { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }

        public IEnumerable<SelectListItem> Readers { get; set; }
    }
}
