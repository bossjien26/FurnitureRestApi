
using System.Collections.Generic;

namespace Services.Dto
{
    public class InventoryIdBySpecifications
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public InventoryIdBySpecificationContents InventoryIdBySpecificationContent { get; set; }
    }
}