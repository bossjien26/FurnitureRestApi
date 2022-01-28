using System.Linq;
using System.Threading.Tasks;
using Entities;
using Enum;

namespace Services.Interface
{
    public interface IMetadataService
    {
        Task Insert(Metadata Metadata);

        Task Update(Metadata Metadata);

        Task<Metadata> GetById(int id);

        Task<Metadata> GetByCategoryDetail(MetadataCategoryEnum category, int key);

        IQueryable<Metadata> GetByCategory(MetadataCategoryEnum category);
    }
}