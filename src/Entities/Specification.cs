using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    [Table("Specification")]
    [Comment("規格")]
    public class Specification
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(256)]
        public string Name { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public bool IsDelete { get; set; } = false;

        public virtual ICollection<SpecificationContent> SpecificationContent { get; set; }

        public virtual ICollection<ProductSpecification> ProductSpecifications { get; set; }
    }
}