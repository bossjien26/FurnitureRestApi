using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enum;

namespace Entities
{
    public class OrderDelivery
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public DeliveryTypeEnum Type { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; }
    }
}