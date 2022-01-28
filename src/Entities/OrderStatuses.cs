using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enum;

namespace Entities
{
    public class OrderStatuses
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Processing;

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }
        

        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; }
    }
}