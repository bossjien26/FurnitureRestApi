using DbEntity;
using Entities;
using Repositories.Interface;

namespace Repositories
{
    public class OrderStatusesRepository : GenericRepository<OrderStatuses>, IOrderStatusesRepository
    {
        public OrderStatusesRepository(DbContextEntity context) : base(context)
        {

        }
    }
}