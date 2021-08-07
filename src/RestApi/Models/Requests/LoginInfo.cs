using System.ComponentModel.DataAnnotations;

namespace RestApi.src.Models
{
    public class AuthenticateRequest
    {
        public int Id { get; set; }

        [Required]
        public string Mail { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}