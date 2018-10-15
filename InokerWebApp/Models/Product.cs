using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InokerWebApp.Models
{
    [Table("Product")]
    public partial class Product
    {
        public int ProductId { get; set; }
        public int ModelId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Stock { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public string Dimension { get { return Width + "x" + Height; } }

        [ForeignKey("ModelId")]
        [InverseProperty("Products")]
        public Model Model { get; set; }
    }
}
