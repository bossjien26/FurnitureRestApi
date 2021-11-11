using System.Collections.Generic;

namespace Services.Dto
{
    public class InventoryWithSpecification
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<InventoryWithSpecification> SpecificationContentLists { get; set; }
    }
}