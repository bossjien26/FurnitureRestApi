using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    [Table("Inventory")]
    public class Inventory
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string SKU { get; set; }

        public int Quantity { get; set; }

        public int Sequence { get; set; }

        public int Price { get; set; }

        public DateTime RelateAt { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public bool IsDisplay { get; set; } = true;

        public bool IsDelete { get; set; } = false;

        public virtual ICollection<InventorySpecification> InventorySpecifications { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
    }
}