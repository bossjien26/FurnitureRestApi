using DbEntity;
using Entities;
using Repositories.Interface;

namespace Repositories
{
    public class MetaDataRepository : GenericRepository<MetaData>, IMetaDataRepository
    {
        public MetaDataRepository(DbContextEntity context) : base(context)
        {

        }
    }
}