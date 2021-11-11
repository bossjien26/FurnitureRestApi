using System.Collections.Generic;
using Entities;

namespace Services.Dto
{
    public class ProductInventory
    {
        public Product Product { get; set; }

        public List<Inventory> Inventory { get; set; }
    }
}