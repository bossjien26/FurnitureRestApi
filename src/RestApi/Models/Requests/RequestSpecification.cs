using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Requests
{
    public class RequestSpecification
    {
        [Required]
        public string Name { get; set; }
    }
}