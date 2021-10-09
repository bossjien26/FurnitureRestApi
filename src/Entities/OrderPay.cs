using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enum;

namespace Entities
{
    [Table("OrderPay")]
    public class OrderPay
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public PaymentTypeEnum Terms { get; set; }

        public bool IsPaid { get; set; } = false;

        public int TotalPrice { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; }
    }
}