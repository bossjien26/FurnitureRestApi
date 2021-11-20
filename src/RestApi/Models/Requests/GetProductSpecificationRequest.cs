using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Requests
{
    public class GetProductSpecificationRequest
    {
        [Required]
        public int ProductId { get; set; }

        public int[] Specifications { get; set; } = new int[0];

        public int[] SpecificationContents { get; set; } = new int[0];
    }
}