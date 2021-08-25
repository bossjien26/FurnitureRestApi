using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Requests
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