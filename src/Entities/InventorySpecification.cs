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

        public int SpecificationId { get; set; }

        [ForeignKey(nameof(InventoryId))]
        public virtual Inventory Inventory { get; set; }

        [ForeignKey(nameof(SpecificationId))]
        public virtual Specification Specification { get; set; }
    }
}