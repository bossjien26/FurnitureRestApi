using System.Collections.Generic;
using Entities;

namespace Services.Dto
{
    public class ProductToInventory
    {
        public int InventoryId { get; set; }

        public int Price { get; set; }

        public string ProductName { get; set; }
    }
}