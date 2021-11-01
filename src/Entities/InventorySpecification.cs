using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    [Table("InventorySpecification")]
    [Comment("產品與規格關聯")]
    public class InventorySpecification
    {
        public int Id { get; set; }

        public int InventoryId { get; set; }

        public int SpecificationContentId { get; set; }

        [ForeignKey(nameof(InventoryId))]
        public virtual Inventory Inventory { get; set; }

        [ForeignKey(nameof(SpecificationContentId))]
        public virtual SpecificationContent SpecificationContent { get; set; }
    }
}