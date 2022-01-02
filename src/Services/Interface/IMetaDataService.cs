using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using Enum;

namespace Services.Interface
{
    public interface IMetadataService
    {
        Task Insert(Metadata Metadata);

        void Update(Metadata Metadata);

        Task<Metadata> GetById(int id);

        Metadata GetByCategoryDetail(MetadataCategoryEnum category, int key);

        IEnumerable<Metadata> GetByCategory(MetadataCategoryEnum category);
    }
}