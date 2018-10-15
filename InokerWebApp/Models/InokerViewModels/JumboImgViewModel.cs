using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InokerWebApp.Models.InokerViewModels
{
    public class JumboImgViewModel
    {
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 10)]
        public string Photo { get; set; }
        [Required(ErrorMessage = "Select model")]
        public string Product { get; set; }
    }
}
