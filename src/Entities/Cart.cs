using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Cart")]
    public class Cart
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public byte Attribute { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}