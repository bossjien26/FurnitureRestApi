using DbEntity;
using Entities;
using Repositories.Interface;

namespace Repositories
{
    public class MetadataRepository : GenericRepository<Metadata>, IMetadataRepository
    {
        public MetadataRepository(DbContextEntity context) : base(context)
        {

        }
    }
}