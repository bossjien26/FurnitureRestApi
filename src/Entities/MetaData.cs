using System.ComponentModel.DataAnnotations.Schema;
using Enum;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    [Table("MetaData")]
    [Index(nameof(Category), nameof(Type), nameof(Key))]
    public class MetaData
    {
        public int Id { get; set; }

        public MetaDataCategoryEnum Category { get; set; }

        public int Type { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}