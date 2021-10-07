using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    [Table("UserDetail")]
    [Comment("顧客資訊")]
    public class UserDetail
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(256)]
        public string Country { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(256)]
        public string City { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(256)]
        public string Street { get; set; }

        public DateTime UpdateAt { get; set; } = DateTime.Now;

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}