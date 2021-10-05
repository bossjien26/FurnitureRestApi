using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Requests
{
    public class InventorySpecificationRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int SpecificationId { get; set; }
    }
}