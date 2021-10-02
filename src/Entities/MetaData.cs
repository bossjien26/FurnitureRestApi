using System.ComponentModel.DataAnnotations.Schema;
using Enum;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    [Table("Metadata")]
    [Index(nameof(Category), nameof(Type), nameof(Key))]
    public class Metadata
    {
        public int Id { get; set; }

        public MetadataCategoryEnum Category { get; set; }

        public int Type { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}