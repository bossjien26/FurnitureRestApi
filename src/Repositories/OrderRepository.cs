using DbEntity;
using Entities;
using Repositories.Interface;

namespace Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(DbContextEntity context) : base(context)
        {

        }
    }
}