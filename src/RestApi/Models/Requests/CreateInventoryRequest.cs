using System;

namespace RestApi.Models.Requests
{
    public class CreateInventoryRequest
    {
        public int ProductId { get; set; }

        public string SKU { get; set; }

        public int Quantity { get; set; }

        public int Sequence { get; set; }

        public int Price { get; set; }

        public DateTime RelateAt { get; set; }

        public bool IsDisplay { get; set; }
    }
}