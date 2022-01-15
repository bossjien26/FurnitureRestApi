using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    [Table("Category")]
    [Comment("商品分類")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(256)]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        public int Sequence { get; set; }

        public bool IsDisplay { get; set; } = true;

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public bool IsDelete { get; set; } = false;

        public virtual Category ParentCategory { get; set; }

        public virtual ICollection<Category> ChildrenCategories { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}