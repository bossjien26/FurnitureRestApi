using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    [Table("ProductSpecification")]
    [Comment("產品規格項目")]
    public class ProductSpecification
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int SpecificationId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        [ForeignKey(nameof(SpecificationId))]
        public virtual Specification Specification { get; set; }

        public virtual ICollection<InventorySpecification> InventorySpecifications { get; set; }
    }
}