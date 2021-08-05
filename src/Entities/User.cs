using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Column(TypeName = "VARCHAR")]
        public string Name { get; set; }

        public string Verify { get; set; }

        public string Access { get; set; }

        public DateTime Create { get; set; } = DateTime.Now;

        public bool IsDelete { get; set; }
    }
}