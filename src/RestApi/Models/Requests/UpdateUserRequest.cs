using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Requests
{
    public class UpdateUserRequest
    {
        [Required]
        public string Name { get; set; }
    }
}