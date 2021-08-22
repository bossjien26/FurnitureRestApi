using System.ComponentModel.DataAnnotations;

namespace RestApi.src.Models
{
    public class Authenticate
    {
        public int Id { get; set; }

        [Required]
        public string Mail { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}