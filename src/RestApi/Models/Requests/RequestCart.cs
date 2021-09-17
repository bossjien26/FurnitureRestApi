using System.ComponentModel.DataAnnotations;
using Enum;

namespace RestApi.Models.Requests
{
    public class RequestCart
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public CartAttribute Attribute { get; set; }
    }
}