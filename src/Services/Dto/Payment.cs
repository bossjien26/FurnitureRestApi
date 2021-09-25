using Enum;

namespace Services.Dto
{
    public class Payment
    {
        public string Title { get; set; }

        public PaymentTypeEnum Type { get; set; }

        public string Introduce { get; set; }

        public string Content { get; set; }
    }
}