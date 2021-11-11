using System.Collections.Generic;
using Entities;

namespace Services.Dto
{
    public class InventoryToOrderInventory
    {
        public int InventoryId { get; set; }

        public int Price { get; set; }

        public string ProductName { get; set; }

        public List<Specification> Specifications { get; set; }
        public List<SpecificationContent> SpecificationContents { get; set; }
    }
}