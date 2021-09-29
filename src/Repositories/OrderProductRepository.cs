using DbEntity;
using Entities;
using Repositories.Interface;

namespace Repositories
{
    public class OrderProductRepository : GenericRepository<OrderProduct> , IOrderProductRepository
    {
        public OrderProductRepository(DbContextEntity context):base(context)
        {

        }
    }
}