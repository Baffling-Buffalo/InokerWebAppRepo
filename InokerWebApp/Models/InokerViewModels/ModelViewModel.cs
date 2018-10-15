using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InokerWebApp.Models.InokerViewModels
{
    public class ModelViewModel
    {
        public int ModelId { get; set; }
        [Required(ErrorMessage = "Unesi naziv kolekcije")]
        [Display(Name = "Kolekcija")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Min 2 max 30 karaktera")]
        public string Collection { get; set; }
        [Required(ErrorMessage = "Unesi naziv modela")]
        [Display(Name = "Model")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Min 2 max 30 karaktera")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Unesi sirinu")]
        //[Display(Name = "Sirina")]
        //public int Width { get; set; }
        //[Required(ErrorMessage = "Unesi visinu")]
        //[Display(Name = "Visina")]
        //public int Height { get; set; }
        //[Required(ErrorMessage = "Unesi kolicinu na lageru")]
        //[Display(Name = "Kolicina na lageru")]
        //public int Stock { get; set; }
        //[Required(ErrorMessage = "Unesi cenu")]
        //[Display(Name = "Cena")]
        //public decimal Price { get; set; }

        // base64 strings
        [Required(ErrorMessage = "Izaberi sliku proizvoda")]
        [Display(Name = "Slika plocice")]
        [StringLength(int.MaxValue, MinimumLength = 10)]
        public string Photo1 { get; set; }
        [Display(Name = "Slika u prostoru")]
        [StringLength(int.MaxValue, MinimumLength = 10)]
        public string Photo2 { get; set; }
    }
}
