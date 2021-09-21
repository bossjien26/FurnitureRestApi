using DbEntity;
using Entities;
using Repositories.Interface;

namespace Repositories
{
    public class SpecificationRepository : GenericRepository<Specification> , ISpecificationRepository
    {
        public SpecificationRepository(DbContextEntity context) : base(context)
        {

        }
    }
}