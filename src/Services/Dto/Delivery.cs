using Enum;

namespace Services.Dto
{
    public class Delivery
    {
        public string Title { get; set; }

        public DeliveryTypeEnum Type { get; set; }

        public string Introduce { get; set; }

        public string Content { get; set; }
    }
}