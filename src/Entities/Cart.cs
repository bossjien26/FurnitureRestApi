using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Cart
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "Char")]
        [StringLength(15)]
        public string Attribute { get; set; }

        public int Price { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}