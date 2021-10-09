using System.ComponentModel.DataAnnotations;
using Enum;

namespace RestApi.Models.Requests
{
    public class RequestCart
    {
        [Required]
        public int InventoryId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public CartAttributeEnum Attribute { get; set; }
    }
}