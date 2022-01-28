using System.ComponentModel.DataAnnotations;
using Enum;

namespace RestApi.Models.Requests
{
    public class CreateOrderDeliveryRequest
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public DeliveryTypeEnum Type { get; set; }
    }
}