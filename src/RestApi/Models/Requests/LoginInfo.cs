using System.ComponentModel.DataAnnotations;

namespace ResApi.src.Models
{
    public class LoginInfo
    {
        public int Id { get; set; }

        [Required]
        public string Mail { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}