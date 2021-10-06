using Enum;

namespace Entities
{
    public class Cart
    {
        public string UserId { get; set; }

        public string InventoryId { get; set; }

        public string Quantity { get; set; }

        public CartAttributeEnum Attribute { get; set; }
    }
}