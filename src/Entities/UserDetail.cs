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

        [ForeignKey("User")]
        public int UserId { get; set; }

        public string Country { get; set; }

        public string City { get; set; }
        
        public string Street { get; set; }

        public string CrashCard { get; set; }

        public DateTime UpdateAt { get; set; } = DateTime.Now;

        public virtual User User { get; set; }
    }
}