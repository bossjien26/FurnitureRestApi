using DbEntity;
using Entities;
using Repositories.IRepository;
using src.Repositories.Repository;

namespace Repositories.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(DbContextEntity context) : base(context)
        {

        }
    }
}