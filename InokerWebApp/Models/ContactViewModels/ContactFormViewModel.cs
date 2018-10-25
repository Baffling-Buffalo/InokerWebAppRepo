using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InokerWebApp.Models.ContactViewModels
{
    public class ContactFormViewModel
    {
        [Required]
        [Display(Name = "Ime")]
        public string Name { get; set; }
        [Display(Name = "Prezime")]
        public string Prezime { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Poruka")]
        public string Message { get; set; }
    }
}
