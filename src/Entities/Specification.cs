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

        public string Name { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public bool IsDelete { get; set; }

        public virtual SpecificationContent SpecificationContent { get; set; }

        public virtual ICollection<ProductSpecification> ProductSpecifications { get; set; }
    }
}