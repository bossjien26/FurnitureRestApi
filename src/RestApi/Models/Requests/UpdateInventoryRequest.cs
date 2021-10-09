using System;

namespace RestApi.Models.Requests
{
    public class UpdateInventoryRequest
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string SKU { get; set; }

        public int Quantity { get; set; }

        public int Sequence { get; set; }

        public int Price { get; set; }

        public DateTime RelateAt { get; set; }

        public bool IsDisplay { get; set; }
    }
}