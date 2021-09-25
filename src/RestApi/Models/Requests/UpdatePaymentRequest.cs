using Enum;

namespace RestApi.Models.Requests
{
    public class UpdatePaymentRequest
    {
        public string Title { get; set; }

        public PaymentTypeEnum Type { get; set; }

        public string Introduce { get; set; }

        public string Content { get; set; }
    }
}