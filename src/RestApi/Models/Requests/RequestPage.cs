using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Requests
{
    public class RequestPage
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Pages { get; set; }
    }
}