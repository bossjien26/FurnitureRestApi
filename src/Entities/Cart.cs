using System;
using System.ComponentModel.DataAnnotations.Schema;
using Enum;

namespace Entities
{
    public class Cart
    {
        public string UserId { get; set; }

        public string ProductId { get; set; }

        public string Quantity { get; set; }
    }
}