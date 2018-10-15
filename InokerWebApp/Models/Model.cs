using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InokerWebApp.Models
{
    [Table("Model")]
    public partial class Model
    {
        public Model()
        {
            JumboImgs = new HashSet<JumboImg>();
            Products = new HashSet<Product>();
        }

        public int ModelId { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        public int CollectionId { get; set; }
        [StringLength(60)]
        public string Description { get; set; }
        [Required]
        public byte[] Photo1 { get; set; }
        [Required]
        public byte[] Photo2 { get; set; }

        [ForeignKey("CollectionId")]
        [InverseProperty("Models")]
        public Collection Collection { get; set; }
        [InverseProperty("Model")]
        public ICollection<JumboImg> JumboImgs { get; set; }
        [InverseProperty("Model")]
        public ICollection<Product> Products { get; set; }
    }
}
