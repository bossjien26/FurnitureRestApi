using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("OrderInventory")]
    public class OrderInventory
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(256)]
        public string ProductName { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(256)]
        public string Specification { get; set; }

        public int Quality { get; set; }

        public int Price { get; set; }

        public virtual Order Order { get; set; }
    }
}