using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Requests
{
    public class Registration
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Mail { get; set; }

        [Required]
        public string Password { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }
    }
}