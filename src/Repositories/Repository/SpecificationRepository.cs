using DbEntity;
using Entities;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class SpecificationRepository : GenericRepository<Specification> , ISpecificationRepository
    {
        public SpecificationRepository(DbContextEntity context) : base(context)
        {

        }
    }
}