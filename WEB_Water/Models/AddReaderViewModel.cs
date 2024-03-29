﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Water.Models
{
    public class AddReaderViewModel
    {
        [Display(Name = "Reader")]
        public string ReaderName { get; set; }

      
        [Display(Name = "Installation date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Installation { get; set; }

        
        [Display(Name = "Address")]
        public string AddressName { get; set; }
        
        [Display(Name = "Customer")]
        public string UserId { get; set; }

        
        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
