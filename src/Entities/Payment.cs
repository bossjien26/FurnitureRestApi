using Enum;

namespace Entities
{
    public class Payment
    {
        public string Title { get; set; }

        public PaymentTypeEnum Terms { get; set; }

        public string Introduce { get; set; }

        public string Content { get; set; }
    }
}