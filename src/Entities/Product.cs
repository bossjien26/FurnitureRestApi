using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(256)]
        public string Name { get; set; }

        public int Quantity { get; set; }

        public int Sequence { get; set; }

        public int Price { get; set; }

        public DateTime RelateAt { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public bool IsDisplay { get; set; } = true;

        public bool IsDelete { get; set; } = false;

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }

        public virtual ICollection<ProductSpecification> ProductSpecifications { get; set; }
    }
}