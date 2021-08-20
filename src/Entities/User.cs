using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enum;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    [Table("User")]
    [Index(nameof(Id))]
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Mail { get; set; }

        public string Password { get; set; }

        [Column(TypeName = "Varchar")]
        [StringLength(256)]
        public string Name { get; set; }

        public bool IsVerify { get; set; } = false;

        public byte Role { get; set; } = (byte)Enum.Role.Customer;

        public string Token { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.Now;

        public bool IsDelete { get; set; } = false;

        public virtual UserDetail UserDetail { get; set; }

        public virtual Cart Cart { get; set; }
    }
}