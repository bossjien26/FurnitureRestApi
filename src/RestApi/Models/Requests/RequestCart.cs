using System.ComponentModel.DataAnnotations;
using Enum;

namespace RestApi.Models.Requests
{
    public class RequestCart
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Quantity { get; set; }

        [Required]
        public CartAttribute Attribute { get; set; }
    }
}