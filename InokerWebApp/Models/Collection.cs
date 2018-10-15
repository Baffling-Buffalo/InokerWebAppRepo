using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InokerWebApp.Models
{
    [Table("Collection")]
    public partial class Collection
    {
        public Collection()
        {
            Models = new HashSet<Model>();
        }

        public int CollectionId { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [InverseProperty("Collection")]
        public ICollection<Model> Models { get; set; }
    }
}
