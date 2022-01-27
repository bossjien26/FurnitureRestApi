using DbEntity;
using Entities;
using Repositories.Interface;

namespace Repositories
{
    public class OrderDeliveryRepository : GenericRepository<OrderDelivery>, IOrderDeliveryRepository
    {
        public OrderDeliveryRepository(DbContextEntity context) : base(context)
        {

        }
    }
}