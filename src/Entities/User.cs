using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace src.Entities
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

        public string EnglishName { get; set; }
    }
}