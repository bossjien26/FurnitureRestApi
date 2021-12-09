using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Requests
{
    public class GenerateAuthenticateRequest
    {
        [Required]
        public string Mail { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}