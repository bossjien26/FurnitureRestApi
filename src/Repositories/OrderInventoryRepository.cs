using DbEntity;
using Entities;
using Repositories.Interface;

namespace Repositories
{
    public class OrderInventoryRepository : GenericRepository<OrderInventory> , IOrderInventoryRepository
    {
        public OrderInventoryRepository(DbContextEntity context):base(context)
        {

        }
    }
}