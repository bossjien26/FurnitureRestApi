using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enum;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    [Table("User")]
    [Index(nameof(Id))]
    [Index(nameof(Mail))]
    [Index(nameof(Password))]
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

        public RoleEnum Role { get; set; } = Enum.RoleEnum.Customer;

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public bool IsDelete { get; set; } = false;

        public virtual UserDetail UserDetail { get; set; }
    }
}