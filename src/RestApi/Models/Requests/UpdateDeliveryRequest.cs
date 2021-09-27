using Enum;

namespace RestApi.Models.Requests
{
    public class UpdateDeliveryRequest
    {
        public string Title { get; set; }

        public DeliveryTypeEnum Type { get; set; }

        public string Introduce { get; set; }

        public string Content { get; set; }
    }
}