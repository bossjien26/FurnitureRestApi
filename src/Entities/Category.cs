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

        public string Name { get; set; }

        public int ChildrenId { get; set; }

        public int Sequence { get; set; }

        public bool IsDisplay { get; set; }

        public DateTime CreateAt { get; set; }

        public bool IsDelete { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories{get;set;}
    }
}