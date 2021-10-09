using Enum;

namespace RestApi.Models.Requests
{
    public class CreateOrderPayRequest
    {
        public int orderId { get; set; }

        public PaymentTypeEnum Terms { get; set; }
    }
}