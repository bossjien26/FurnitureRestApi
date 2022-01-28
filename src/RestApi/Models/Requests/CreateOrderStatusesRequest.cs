using System.ComponentModel.DataAnnotations;
using Enum;

namespace RestApi.Models.Requests
{
    public class CreateOrderStatusesRequest
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public OrderStatusEnum Status { get; set; }
    }
}