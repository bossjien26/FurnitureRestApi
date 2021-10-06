using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enum;

namespace Entities
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Processing;

        [Column(TypeName = "Varchar")]
        [StringLength(150)]
        public string Country { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(150)]
        public string City { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(150)]
        public string Street { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(150)]
        public string Recipient { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(256)]
        public string RecipientMail { get; set; }


        [Column(TypeName = "Varchar")]
        [StringLength(256)]
        public string RecipientPhone { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(150)]
        public string Sender { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public DateTime UpdateAt { get; set; } = DateTime.Now;

        public virtual User User { get; set; }

        public virtual OrderPay OrderPay { get; set; }

        public virtual ICollection<OrderInventory> OrderInventories { get; set; }
    }
}