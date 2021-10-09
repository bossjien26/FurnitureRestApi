using DbEntity;
using Entities;
using Repositories.Interface;

namespace Repositories
{
    public class InventorySpecificationRepository : GenericRepository<InventorySpecification> , IInventorySpecificationRepository
    {
        public InventorySpecificationRepository(DbContextEntity context) : base(context)
        {

        }
    }
}