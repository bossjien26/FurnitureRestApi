using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Requests
{
    public class ShowInventoryWithSpecificationRequest
    {
        [Required]
        public int[] InventoryIds { get; set; }
    }
}