using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    [Table("SpecificationContent")]
    [Comment("規格內容")]
    public class SpecificationContent
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(256)]
        public string Name { get; set; }

        [ForeignKey("Specification")]
        public int SpecificationId { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public bool IsDelete { get; set; } = false;

        public virtual Specification Specification { get; set; }
    }
}