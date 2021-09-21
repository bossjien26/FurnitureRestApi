using DbEntity;
using Entities;
using Repositories.IRepository;

namespace Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(DbContextEntity context) : base(context)
        {

        }
    }
}