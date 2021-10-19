using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Requests
{
    public class CreateSpecificationRequest
    {
        [Required]
        public string Name { get; set; }
    }
}