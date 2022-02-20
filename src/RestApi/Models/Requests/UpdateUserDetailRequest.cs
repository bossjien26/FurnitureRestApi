using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Requests
{
    public class UpdateUserDetailRequest
    {
        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Street { get; set; }
    }
}