using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Requests
{
    public class CreateProductCategoryRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}