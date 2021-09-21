using DbEntity;
using Entities;
using Repositories.Interface;

namespace Repositories
{
    public class SpecificationContentRepository : GenericRepository<SpecificationContent> , ISpecificationContentRepository
    {
        public SpecificationContentRepository(DbContextEntity context) : base(context)
        {
            
        }
    }
}