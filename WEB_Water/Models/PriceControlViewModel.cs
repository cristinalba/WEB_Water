using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Water.Models
{
    public class PriceControlViewModel
    {
        [Display(Name = "Sections")]
        public int Section { get; set; }

        [Display(Name = "Lower Bound (m³)")]
        public int LowerBound { get; set; }

        [Display(Name = "Upper Bound (m³)")]
        public int UpperBound { get; set; }

        [Display(Name = "Price (€)")]
        public double Price { get; set; }


    }
}
