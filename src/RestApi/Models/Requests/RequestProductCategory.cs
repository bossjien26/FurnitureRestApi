using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Requests
{
    public class RequestProductCategory
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}