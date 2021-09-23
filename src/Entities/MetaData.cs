using Enum;

namespace Entities
{
    public class MetaData
    {
        public int Id { get; set; }

        public MetaDataCategoryEnum Category { get; set; }

        public string Type { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}