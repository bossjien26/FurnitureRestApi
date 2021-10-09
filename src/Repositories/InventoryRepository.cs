using DbEntity;
using Entities;
using Repositories.Interface;

namespace Repositories
{
    public class InventoryRepository : GenericRepository<Inventory>, IInventoryRepository
    {
        public InventoryRepository(DbContextEntity context) : base(context)
        {

        }
    }
}