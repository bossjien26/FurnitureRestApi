using System.ComponentModel.DataAnnotations;
using Enum;

namespace RestApi.Models.Requests
{
    public class CreateOrderPayRequest
    {
        [Required]
        public int orderId { get; set; }

        [Required]
        public PaymentTypeEnum Terms { get; set; }
    }
}