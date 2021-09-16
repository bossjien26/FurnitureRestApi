using Enum;

namespace Entities
{
    public class Cart
    {
        public string UserId { get; set; }

        public string ProductId { get; set; }

        public string Quantity { get; set; }

        public CartAttribute Attribute { get; set; }
    }
}