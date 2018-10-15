using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InokerWebApp.Models
{
    [Table("JumboImg")]
    public partial class JumboImg
    {
        public int JumboImgId { get; set; }
        [Required]
        public byte[] Photo { get; set; }
        public int ModelId { get; set; }

        [ForeignKey("ModelId")]
        [InverseProperty("JumboImgs")]
        public Model Model { get; set; }
    }
}
