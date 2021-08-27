using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Requests
{
    public class Category
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int ChildrenId { get; set; }

        public int Sequence { get; set; }

        [Required]
        public bool IsDisplay { get; set; } = false;
    }
}