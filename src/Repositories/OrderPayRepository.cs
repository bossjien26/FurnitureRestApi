using DbEntity;
using Entities;
using Repositories.Interface;

namespace Repositories
{
    public class OrderPayRepository : GenericRepository<OrderPay>, IOrderPayRepository
    {
        public OrderPayRepository(DbContextEntity context) : base(context)
        {

        }
    }
}