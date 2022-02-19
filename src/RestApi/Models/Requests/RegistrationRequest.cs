using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Requests
{
    public class RegistrationRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Mail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}