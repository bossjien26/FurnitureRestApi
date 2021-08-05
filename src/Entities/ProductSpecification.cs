using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    [Table("ProductSpecification")]
    [Comment("產品與規格關聯")]
    public class ProductSpecification
    {
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("Specification")]
        public int SpecificationId { get; set; }

        public virtual Product Product { get; set; }

        public virtual Specification Specification { get; set; }
    }
}