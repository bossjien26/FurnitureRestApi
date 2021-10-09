using System;
using System.ComponentModel.DataAnnotations;

namespace RestApi.Models.Requests
{
    public class RequestProduct
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}