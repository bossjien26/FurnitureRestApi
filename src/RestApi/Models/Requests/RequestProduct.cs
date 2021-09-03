using System;
using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Requests
{
    public class RequestProduct
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int Sequence { get; set; }

        [Required]
        public int Price { get; set; }

        public DateTime RelateAt { get; set; } = DateTime.Now;

        [Required]
        public bool IsDisplay { get; set; } = true;
    }
}