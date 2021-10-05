using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    [Table("InventorySpecification")]
    [Comment("產品與規格關聯")]
    public class InventorySpecification
    {
        public int Id { get; set; }

        [ForeignKey("Inventory")]
        public int InventoryId { get; set; }

        [ForeignKey("Specification")]
        public int SpecificationId { get; set; }

        public virtual Inventory Inventory { get; set; }

        public virtual Specification Specification { get; set; }
    }
}