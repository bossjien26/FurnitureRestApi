using DbEntity;
using Entities;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class SpecificationContentRepository : GenericRepository<SpecificationContent> , ISpecificationContentRepository
    {
        public SpecificationContentRepository(DbContextEntity context) : base(context)
        {
            
        }
    }
}